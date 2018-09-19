using ItLabs.MBox.Contracts.Dtos;
using ItLabs.MBox.Contracts.Entities;
using ItLabs.MBox.Contracts.Enums;
using ItLabs.MBox.Contracts.Interfaces;
using ItLabs.MBox.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ItLabs.MBox.Common.Extentions;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ItLabs.MBox.Domain.Managers
{
    public class RecordLabelManager : BaseManager<RecordLabel>, IRecordLabelManager
    {
        private readonly IRepository _repository;
        private readonly IEmailsManager _emailsManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecordLabelManager(IRepository repository, IEmailsManager emailsManager, UserManager<ApplicationUser> userManager) : base(repository)
        {
            _repository = repository;
            _emailsManager = emailsManager;
            _userManager = userManager;
        }

        public IList<RecordLabel> GetAllRecordLabels()
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}").ToList();
        }

        public IList<RecordLabel> GetNextRecordLabels(int toSkip, int toTake)
        {
            return _repository.GetAll<RecordLabel>(
                includeProperties: $"{nameof(RecordLabel.User)},{nameof(RecordLabel.RecordLabelArtists)}", skip: toSkip, take: toTake).ToList();
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
            prepareAndSendMails(artists.ToList(), recordLabel);

        }

        public IList<RecordLabel> GetSearchedRecordLabels(string searchValue, int toSkip, int toTake)
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
                var validator = new EmailAddressAttribute();
                int iteration = 0;
                while ((result = reader.ReadLine()) != null)
                {
                    var parts = result.Split(",");
                    if (parts[0] == null || parts[1] == null || parts.Length > 2)
                    {
                        addMultipleArtistsDto.Errors.Add("Invalid format detected(has to be: Artist Email, Artist Name), row(s): " + iteration);
                        return addMultipleArtistsDto;
                    }
                    var email = parts[0];
                    var name = parts[1];
                    if (email.Length > 320)
                    {
                        addMultipleArtistsDto.Errors.Add("Max length of email (320) exceeded, row(s):  " + iteration);
                        return addMultipleArtistsDto;
                    }
                    if (name.Length > 50)
                    {
                        addMultipleArtistsDto.Errors.Add("Max length of Artist Name (50) exceeded, row(s):  " + iteration);
                        return addMultipleArtistsDto;
                    }
                    if (!validator.IsValid(email))
                    {
                        addMultipleArtistsDto.Errors.Add("Invalid Email format (example@example.com), row(s): " + iteration);
                        return addMultipleArtistsDto;
                    }
                    if (_userManager.FindByEmailAsync(email).Result != null)
                    {
                        iteration++;
                        continue;
                    }
                    if (_repository.Get<RecordLabelArtist>(filter: x => x.RecordLabel.Id == recordLabelId).Count() > 50)
                    {
                        addMultipleArtistsDto.Errors.Add("Artist limit (50) exceeded");
                        return addMultipleArtistsDto;
                    }
                    var response = _userManager.CreateUser(name, email, Role.Artist);
                    if(response == null)
                    {
                        addMultipleArtistsDto.Errors.Add("Artist could not be created, try again!");
                        return addMultipleArtistsDto;
                    }
                    var userCreated = response.Result;
                    var artist = new Artist() { User = userCreated};
                    addMultipleArtistsDto.ArtistsToBeAdded.Add(artist);
                    iteration++;
                }
            }
            return addMultipleArtistsDto;
        }

        public int CreateMultipleArtists(IList<Artist> artistsToBeAdded, int recordLabelId)
        {
            int counter = 0;
            foreach (var artist in artistsToBeAdded)
            {
                _repository.Create<Artist>(artist, recordLabelId);
                _repository.Create<RecordLabelArtist>(new RecordLabelArtist() { Artist = artist, RecordLabel = _repository.GetById<RecordLabel>(recordLabelId) }, recordLabelId);
                counter++;
            }
            _repository.Save();
            return counter;
        }

        public void prepareAndSendMails(IList<Artist> artists , ApplicationUser recordLabel)
        {
            //prepare the emails set the dto
            IList<MailDto> mailingList = new List<MailDto>();
            var template = _repository.GetAll<EmailTemplate>().Where(c => c.Type == EmailTemplateType.DeletedArtist).FirstOrDefault();
            var mailDto = new MailDto();
            foreach (var artist in artists)
            {
                mailDto.EmailAddress = artist.User.Email;
                mailDto.Subject = template.Subject;
                mailDto.Body = template.Body.Replace("[Name]", artist.User.Name);
                mailingList.Add(mailDto);
            }
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
                _emailsManager.SendMultipleMailSmtp(mailingList, configuration);
            }).Start();
            
            
        }
    }
}