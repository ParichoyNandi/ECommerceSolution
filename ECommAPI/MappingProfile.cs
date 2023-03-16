using AutoMapper;
using ECommAPI.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCenterMap, ProductCenterMapDto>();
            CreateMap<ProductFeePlan, ProductFeePlanDto>();
            CreateMap<Plan, PlanDto>();
            CreateMap<PlanConfig, PlanConfigDto>();
            CreateMap<PlanProduct, PlanProductDto>();
            CreateMap<PlanDetails, PlanDetailsDto>();
            CreateMap<HighestEducationQualification, HighestEducationQualificationDto>();
            CreateMap<Registration, CreateRegistrationDto>();
            CreateMap<CreateRegistrationDto, Registration>();
            CreateMap<CreateRegistrationCourseDto, RegistrationCourse>();
            CreateMap<Config, GetConfigDto>();
            CreateMap<CreateProductConfigMapDto, ProductConfig>();
            CreateMap<ProductConfig, GetProductConfigMapDto>();
            CreateMap<CreateConfigDto, Config>();
            CreateMap<Gender, GetGenderDto>();
            CreateMap<ExamCategory, GetExamCategoryDto>();
            CreateMap<Coupon, GetCouponDto>();
            CreateMap<CouponCategory, GetCouponCategoryDto>();
            CreateMap<CouponType, GetCouponTypeDto>();
            CreateMap<DiscountScheme, GetDiscountSchemeDto>();
            CreateMap<DiscountSchemeDetail, GetDiscountSchemeDetailDto>();
            CreateMap<PlanCoupon, GetPlanCouponDto>();
            CreateMap<CreateDiscountSchemeDto, DiscountScheme>();
            CreateMap<CreateDiscountSchemeDetailDto, DiscountSchemeDetail>();
            CreateMap<DiscountSchemeCenterMap, GetDiscountSchemeCenterMap>();
            CreateMap<DiscountSchemeCenterCourseMap, GetDiscountSchemeCenterCourseMap>();
            CreateMap<FeeComponent, GetFeeComponentDto>();
            CreateMap<Brand, GetBrandDto>();
            //CreateMap<CreateCouponDto, Coupon>()
            //    .ForMember(dest =>
            //            dest.CategoryDetails.CouponCategoryID,
            //            opt => opt.MapFrom(src => src.CouponCategoryID))
            //    .ForMember(dest =>
            //            dest.CouponTypeDetails.CouponTypeID,
            //            opt => opt.MapFrom(src => src.CouponTypeID))
            //    .ForMember(dest =>
            //            dest.DiscountDetails.DiscountSchemeID,
            //            opt => opt.MapFrom(src => src.DiscountID));
            CreateMap<Centre, GetCentreDto>();
            CreateMap<Course, GetCourseDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<CreatePlanPaymentDto, PlanPayment>();
            CreateMap<CreateProductPaymentDto, ProductPayment>();
            CreateMap<ValidateCoupon, ValidateCouponResponseDto>();
            CreateMap<GetBatchListRequestDto, GetBatchListResponseDto>();
            CreateMap<Batch, GetBatchDto>();
            CreateMap<CreateSubscriptionDto, ProductSubscription>();
            CreateMap<StudentSubscription, GetStudentSubscriptionDto>();
            CreateMap<StudentSubscriptionFeeSchedule, GetStudentSubscriptionFeeScheduleDto>();
            CreateMap<StudentSubscriptionDue, GetStudentSubscriptionDueDto>()
                .ForMember(dest =>
                        dest.PayableAmount,
                        opt => opt.MapFrom(src => src.FinalPayableAmount))
                .ForMember(dest =>
                        dest.PayableTax,
                        opt => opt.MapFrom(src => src.FinalPayableTax));
            CreateMap<CreateStudentSubscriptionPaymentDto, StudentSubscriptionPaymentResponseDto>();
            CreateMap<StudentSubscriptionPaymentResponseDto, StudentSubscriptionTransaction>();
            CreateMap<RegistrationCourseEnrolment, GetRegistrationCourseEnrolmentDto>();
            CreateMap<FeeStructure, GetFeeStructureDto>();
            CreateMap<FeeStructureTax, GetFeeStructureTaxDto>();
            CreateMap<InstalmentSummary, GetInstalmentSummaryDto>();
            CreateMap<StudentPayOut, GetStudentPayOutDto>();
            CreateMap<StudentPayOutFeeSchedule, GetStudentPayOutFeeScheduleDto>();
            CreateMap<CreateStudentPayOutPaymentDto, StudentPayOutPaymentResponseDto>();
            CreateMap<CreateSubscriptionFeeSchedulePaymentDto, StudentPayOutPaymentFeeScheduleResponseDto>();
            CreateMap<StudentPayOutPaymentResponseDto, StudentPayOutTransaction>();
            CreateMap<StudentPayOutPaymentFeeScheduleResponseDto, StudentPayoutTransactionFeeSchedule>();
            CreateMap<Registration, GetRegistrationMissingDetailsDto>();
            CreateMap<UpdateRegistrationDto, Registration>();
            CreateMap<SocialCategory, GetSocialCategoryDto>();
            CreateMap<ProductSubscription, GetCustomerMandateLinkDto>();
            CreateMap<FeePlan, GetFeePlanDto>();
            CreateMap<CreatePayOutFeeSchedulePaymentDto, StudentPayOutPaymentFeeScheduleResponseDto>();
            CreateMap<CreateSubscriptionFeeSchedulePaymentDto, StudentSubscriptionPaymentFeeScheduleResponseDto>();
            CreateMap<CreatePlanDto, Plan>();
            CreateMap<CreatePlanConfigMapDto, PlanConfig>();
            CreateMap<PlanConfig, GetPlanConfigMapDto>();
            CreateMap<Order, GetOrderDto>();
            CreateMap<OrderDetail, GetOrderDetailDto>();
            CreateMap<FAQ, GetFAQDto>();
            CreateMap<FAQDetail, GetFAQDetailDto>();
            CreateMap<AboutBrand, GetAboutBrandDto>();
            CreateMap<ExamGroup, GetExamGroupDto>();
            CreateMap<CourseEnrolment, GetCourseEnrolmentDto>();
            CreateMap<IssueCategory, GetIssueCategoryDto>();
            CreateMap<Coupon, GetCouponDetailsWithCustomerIdDto>();
            CreateMap<Campaign, GetCampaignDetailsDto>();
            CreateMap<CampaignDiscountMap, GetCampaignDiscountMapDetailsDto>();
            CreateMap<DiscountScheme, GetCampaignDiscountSchemeDto>();
            CreateMap<RecommendedCourseProductPlan, GetRecommendedCourseProductPlanDto>();
            CreateMap<ProductPlan, GetProductPlanDto>();
            CreateMap<StudentRegDetails, GetUpdatedStudentRegDetailsDto>();
            CreateMap<UpdateStudentProfileDto,Registration>();
            
        }
    }
}
