using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ItLabs.MBox.Common.Extensions;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelManager : BaseManager<RecordLabel>, IRecordLabelManager
    {
        private readonly IRepository _repository;
        private readonly IEmailsManager _emailsManager;
        private readonly IS3Manager _s3Manager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecordLabelManager(ILogger<RecordLabel> logger, IRepository repository, IEmailsManager emailsManager, UserManager<ApplicationUser> userManager, IS3Manager s3Manager) : base(repository, logger)
        {
            _repository = repository;
            _emailsManager = emailsManager;
            _userManager = userManager;
            _s3Manager = s3Manager;
        }

        public void DeleteRecordLabel(ApplicationUser user)
        {
            var recordLabelArtists = _repository.Get<RecordLabelArtist>(filter: x => x.RecordLabel.User == user, includeProperties: $"{nameof(Artist)}.{nameof(Artist.User)},{nameof(RecordLabel)}");
            var artists = recordLabelArtists.Select(x => x.Artist);
            foreach (var artist in artists)
            {
                artist.IsDeleted = true;

                _repository.Update<Artist>(artist, user.Id);
            }
            foreach (var rla in recordLabelArtists)
            {
                _repository.Delete(rla);
            }

            var recordLabel = user;

            _repository.Delete<RecordLabel>(user.Id);
            _repository.Delete(user);
            _repository.Save();
            PrepareAndSendMails(artists.ToList(), recordLabel);

        }

        public IList<RecordLabel> SearchRecordLabels(string searchValue, int toSkip, int toTake)
        {
            return _repository.Get<RecordLabel>(
                filter: x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()) || x.User.Email.ToUpper().Contains(searchValue.ToUpper()),
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}",
                skip: toSkip, take: toTake
                ).ToList();
        }
        public AddMultipleArtistsDto ValidateCsvFile(IFormFile formFile, int recordLabelId)
        {
            var addMultipleArtistsDto = new AddMultipleArtistsDto();
            var errorsRows = new Dictionary<string, List<int>>();

            if (formFile.ContentType != "application/vnd.ms-excel")
            {
                addMultipleArtistsDto.Errors.Add("Uploaded file must be in .csv format");
                return addMultipleArtistsDto;
            }

            if (!(formFile.Length > 0))
            {
                addMultipleArtistsDto.Errors.Add("Csv file is empty!");
                return addMultipleArtistsDto;
            }

            var result = string.Empty;
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                int iteration = 1;
                while ((result = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
                    {
                        iteration++;
                        continue;
                    }
                        
                    var parts = result.Split(",");
                    parts = parts.Where(str => str != "").ToArray();
                    if (parts.Length == 0)
                    {
                        iteration++;
                        continue;
                    }
                    if (parts.Length > 2 || parts.Length < 2)
                    {
                        if (!errorsRows.ContainsKey("InvalidFormat"))
                        {
                            errorsRows.Add("InvalidFormat", new List<int>() { iteration });
                        }
                        else
                        {
                            errorsRows.TryGetValue("InvalidFormat", out List<int> rows);
                            rows.Add(iteration);
                            errorsRows["InvalidFormat"] = rows;
                        }
                        iteration++;
                        continue;
                    }
                    var email = parts[0];
                    var name = parts[1];
                    if (email.Length > 320)
                    {
                        if (!errorsRows.ContainsKey("MaxLengthEmail"))
                        {
                            errorsRows.Add("MaxLengthEmail", new List<int>() { iteration });
                        }
                        else
                        {
                            errorsRows.TryGetValue("MaxLengthEmail", out List<int> rows);
                            rows.Add(iteration);
                            errorsRows["MaxLengthEmail"] = rows;
                        }
                    }
                    if (name.Length > 50)
                    {
                        if (!errorsRows.ContainsKey("MaxLengthName"))
                        {
                            errorsRows.Add("MaxLengthName", new List<int>() { iteration });
                        }
                        else
                        {
                            errorsRows.TryGetValue("MaxLengthName", out List<int> rows);
                            rows.Add(iteration);
                            errorsRows["MaxLengthName"] = rows;
                        }
                    }
                    if (!Regex.Match(email, @"^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$").Success)
                    {
                        if (!errorsRows.ContainsKey("InvalidEmailFormat"))
                        {
                            errorsRows.Add("InvalidEmailFormat", new List<int>() { iteration });
                        }
                        else
                        {
                            errorsRows.TryGetValue("InvalidEmailFormat", out List<int> rows);
                            rows.Add(iteration);
                            errorsRows["InvalidEmailFormat"] = rows;
                        }
                    }
                    if (_userManager.FindByEmailAsync(email).Result != null)
                    {
                        iteration++;
                        continue;
                    }
                    var response = new ApplicationUser() { Name = name, Email = email };
                    addMultipleArtistsDto.UsersToBeAdded.Add(response);
                    iteration++;
                }

                if (errorsRows.ContainsKey("InvalidFormat"))
                {
                    string errorString = "Invalid format detected(has to be: Artist Email, Artist Name), row(s): ";
                    foreach (var row in errorsRows["InvalidFormat"])
                    {
                        errorString = errorString + row + ", ";
                    }
                    addMultipleArtistsDto.Errors.Add(errorString.TrimEnd().TrimEnd(','));
                }
                if (errorsRows.ContainsKey("MaxLengthEmail"))
                {
                    string errorString = "Max length of email (320) exceeded, row(s): ";
                    foreach (var row in errorsRows["MaxLengthEmail"])
                    {
                        errorString = errorString + row + ", ";
                    }

                    addMultipleArtistsDto.Errors.Add(errorString.TrimEnd().TrimEnd(','));
                }
                if (errorsRows.ContainsKey("MaxLengthName"))
                {
                    string errorString = "Max length of Artist Name (50) exceeded, row(s): ";
                    foreach (var row in errorsRows["MaxLengthName"])
                    {
                        errorString = errorString + row + ", ";
                    }

                    addMultipleArtistsDto.Errors.Add(errorString.TrimEnd().TrimEnd(','));
                }

                if (errorsRows.ContainsKey("InvalidEmailFormat"))
                {
                    string errorString = "Invalid Email format (example@example.com), row: ";
                    foreach (var row in errorsRows["InvalidEmailFormat"])
                    {
                        errorString = errorString + row + ", ";
                    }

                    addMultipleArtistsDto.Errors.Add(errorString.TrimEnd().TrimEnd(','));
                }
            }
            return addMultipleArtistsDto;
        }

        public List<Artist> CreateMultipleArtists(IList<ApplicationUser> usersToBeAdded, int recordLabelId)
        {
            var artistList = new List<Artist>();
            foreach (var user in usersToBeAdded)
            {
                var returned = _userManager.CreateUser(user.Name, user.Email, Role.Artist);
                if (returned == null)
                {
                    foreach(var users in usersToBeAdded)
                    {
                        if(_userManager.FindByEmailAsync(users.Email).Result != null)
                        {
                            var toDelete = _repository.GetOne<Artist>(filter: x => x.User == users);
                            if(toDelete != null)
                            {
                                _repository.Delete(toDelete);
                            }
                            _repository.Delete(users);
                            _repository.Save();
                        }
                    }
                    
                    return null;
                }
                var addedUser = returned.Result;
                var artist = new Artist() { User = addedUser, RecordLabelArtists = new List<RecordLabelArtist> { new RecordLabelArtist { RecordLabelId = recordLabelId } } };
                artistList.Add(artist);
                _repository.Create<Artist>(artist, recordLabelId);
                _repository.Save();
            }

            return artistList;
        }

        private void PrepareAndSendMails(IList<Artist> artists, ApplicationUser recordLabel)
        {
            //prepare the emails set the dto
            IList<MailDto> mailingList = new List<MailDto>();
            var template = _repository.GetAll<EmailTemplate>().Where(c => c.Type == EmailTemplateType.DeletedArtist).FirstOrDefault();

            foreach (var artist in artists)
            {
                var artistDto = new MailDto();
                artistDto.EmailAddress = artist.User.Email;
                artistDto.Subject = template.Subject;
                artistDto.Body = template.Body.Replace("[Name]", artist.User.Name);
                mailingList.Add(artistDto);
            }
            var mailDto = new MailDto();
            template = _repository.GetAll<EmailTemplate>().Where(c => c.Type == EmailTemplateType.DeletedRecordLabel).FirstOrDefault();
            mailDto.EmailAddress = recordLabel.Email;
            mailDto.Subject = template.Subject;
            mailDto.Body = template.Body.Replace("[Name]", recordLabel.Name);
            mailingList.Add(mailDto);

            var configuration = _repository.GetAll<Configuration>().ToList();
            //fire and forget
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                _emailsManager.SendMultipleMails(mailingList, configuration);
            }).Start();


        }
        public int GetNumberOfArtists(int recordLabelId)
        {
            return _repository.GetCount<RecordLabelArtist>(filter: x => x.RecordLabel.Id == recordLabelId);
        }
    }
}