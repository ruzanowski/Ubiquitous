using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SmartStore.Persistance.Context;
using U.SmartStoreAdapter.Application.Models.Pictures;
using U.SmartStoreAdapter.Domain.Entities.Media;

namespace U.SmartStoreAdapter.Application.Operations.Pictures
{
    public class StorePicturesCommandHandler : IRequestHandler<StorePicturesCommand,int>
    {
        private readonly SmartStoreContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly HttpClient _httpClient;

        public StorePicturesCommandHandler(SmartStoreContext context, IMapper mapper, IMediator mediator, HttpClient httpClient)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _httpClient = httpClient;
        }
        
        public async Task<int> Handle(StorePicturesCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var mediaStorageId = await SavePictureAsMediaStorage(request, cancellationToken);
                    var pictureId = await SavePicture(request, mediaStorageId, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();
                    return pictureId;
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        private async Task<int> SavePicture(StorePicturesCommand request, int mediaStorageId, CancellationToken cancellationToken)
        {
            var picture = new Picture
            {
                MediaStorageId = mediaStorageId,
                MimeType = request.MimeType,
                IsNew = true,
                IsTransient = false,
                SeoFilename = request.SeoFileName,
                UpdatedOnUtc = DateTime.UtcNow
            };
                
            await _context.AddAsync(picture, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return picture.Id;
        }
        
        private async Task<int> SavePictureAsMediaStorage(StorePicturesCommand request, CancellationToken cancellationToken)
        {
            var pictureBinary = await DownloadAttachment(request.Url);
            var mediaStorage = new MediaStorage
            {
                Data = pictureBinary
            };
            await _context.MediaStorages.AddAsync(mediaStorage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return mediaStorage.Id;
        }

        private async Task<Byte[]> DownloadAttachment(string uri)
        {
            return await _httpClient.GetByteArrayAsync(@uri);
        }
    }
}