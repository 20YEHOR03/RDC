using AutoMapper;
using RDC.API.Models.Company;
using RDC.API.Models.Drone;
using RDC.API.Models.Flight;
using RDC.API.Models.Subscription;
using RDC.API.Models.SubscriptionPayment;
using RDC.API.Models.User;

namespace RDC.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegister, UserModel>();
            CreateMap<UserModel, UserRegister>();

            CreateMap<CompanyModel, CompanyDTO>();
            CreateMap<CompanyDTO, CompanyModel>();

            CreateMap<DroneModel, DroneDTO>();
            CreateMap<DroneDTO, DroneModel>();

            CreateMap<FlightModel, FlightDTO>();
            CreateMap<FlightDTO, FlightModel>();

            CreateMap<SubscriptionModel, SubscriptionDTO>();
            CreateMap<SubscriptionDTO, SubscriptionModel>();

            CreateMap<SubscriptionPaymentModel, SubscriptionPaymentDTO>();
            CreateMap<SubscriptionPaymentDTO, SubscriptionPaymentModel>();
        }
    }
}
