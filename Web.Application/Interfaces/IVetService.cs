using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.Common;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.DTOs.VetDTO.DownloadReceipt;
using Web.Application.Response;

namespace Web.Application.Interfaces
{
    public interface IVetService
    {
        Task<BaseResponse<VetDetailsDto>> AddAsync(string userId, VetRequest request);
        Task<BaseResponse<PaginatedList<VetListItemDto>>> GetAllAsync(RequestFilters filters = default!,AddtionalRequestFilters addtionalRequestFilters=default!);
        Task<BaseResponse<VetDetailsDto>> GetAsync(int id);
        Task<BaseResponse<List<AvailableSlotDto>>> GetAvailableSlotsAsync(int VetId,GetAvailableSlotsRequest request);
        Task<BaseResponse<VetReviewsDto>> GetReviewsasync(int VetId);
        Task<BaseResponse<VetBookingReceiptDTO>> BookingVet(string userid,int VetclinicId, BookVetDTO request);
        Task<BaseResponse<bool>> ConfirmBookingAsync(string userid,int bookingId);
        Task<BaseResponse<ReceiptPdfResult?>> GenerateVetReceiptPdfByBookingIdAsync(int bookingId);
        Task<BaseResponse<bool>> UpdateAsync(int id, VetRequest request);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
