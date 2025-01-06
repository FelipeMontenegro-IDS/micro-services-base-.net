using Application.Features.Customers.Commands.CreateCustomer;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region Commands

        CreateMap<CreateCustomerCommand, Customer>();

        #endregion

        #region Queries

        #endregion
    }
}