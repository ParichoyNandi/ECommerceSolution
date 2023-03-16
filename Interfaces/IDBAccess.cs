using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Interfaces
{
    public interface IDBAccess
    {
        List<Category> GetCategories();
        List<Product> GetProducts(int BrandID, int CenterID = 0, int CategoryID = 0, bool IsPublished = true);
        List<Plan> GetPlanList(string BrandIDList, int CategoryID, string ExamCategoryIDs = null, int CouponID = 0, string ExamGroupIDs = null, int ProductID = 0, string CustomerID = null);
        List<PlanProduct> GetPlanProductDetails(int PlanID);
        List<HighestEducationQualification> GetHighestEducationQualificationList(int BrandID);
        int SaveRegistration(Registration reg);
        List<Config> GetConfigs();
        ProductConfig SaveProductConfig(ProductConfig config);
        int SaveConfigDetails(Config config);
        List<Gender> GetGenders();
        List<ExamCategory> GetExamCategories(int BrandID);
        List<Coupon> GetCouponsForPlan(int PlanID);
        ValidateCoupon ValidatePlanCoupon(int BrandID, string CouponCode, int PlanID, int ProductID, int PaymentMode, int CenterID, ValidateCoupon obj,
                                                    decimal PurchaseAmount = 0, string CustomerID = null);
        int SaveDiscountScheme(DiscountScheme discountScheme, string BrandList, string CreatedBy);
        int SaveDiscountSchemeDetails(int DiscountSchemeID, DiscountSchemeDetail discountScheme, string CreatedBy);
        void SaveDiscountSchemeCenterCourseMap(int DiscountSchemeID, int CenterID, int CourseID, string CreatedBy);
        List<DiscountSchemeCenterMap> GetDiscountSchemeCenterCourseMap(int DiscountSchemeID);
        List<FeeComponent> GetFeeComponents(int BrandID);
        List<Brand> GetBrands();
        int SaveCoupon(Coupon coupon, string BrandList);
        List<Centre> GetCentres(int BrandID = 109);
        List<Course> GetCourses(int BrandID = 109);
        List<DiscountScheme> GetDiscountSchemes(string BrandIDList);
        List<CouponCategory> GetCouponCategories();
        bool ValidatePurchase(Payment payment);
        void SaveTransactionDetails(Payment payment);
        List<Coupon> GetCoupons(int CouponCategoryID, int BrandID = 109);
        int SaveCouponPlanMap(int CouponID, int PlanID, DateTime ValidFrom, DateTime ValidTo);
        int UpdateRegistration(Registration reg);
        int VerifyAdmissionType(int EnquiryID);
        Transaction GetTransactionDetails(string TransactionNo);
        int GetEnquiryDetails(int RegID, int CenterID, int BatchID, decimal ProspectusAmount);
        int GetRollNo(string format);
        int uspInsertStudentDetailsFromEnquiry(int EnquiryID, string StudentCode, string CreatedBy, DataHelper dh, SqlTransaction trans);
        void uspInsertStudentBatchDetails(string CourseXML, string CreatedBy, DataHelper dh, SqlTransaction trans);
        List<FeeStructure> GetFeePlanDetails(int FeePlanID, int PaymentModeID, int DiscountSchemeID, DateTime BatchStartDate);
        int InsertInvoice(string InvoiceXML, int BrandID, DataHelper dh, SqlTransaction trans);
        void GetInvoiceDetail(FeeSchedule objInvoice, DataHelper dh, SqlTransaction trans);
        DateTime FindNxtInstalmentDate(int receiptid);
        void GetReceipt(Receipt objReceipt, DataHelper dh, SqlTransaction trans);
        void InsertReceiptObject(Receipt objReceipt, int BrandID, DataHelper helper, SqlTransaction trans);
        void UpdateReceiptDetail(Receipt objReceipt, DataHelper dh, SqlTransaction trans);
        void UpdateTransactionDetails(int TransactionProductDetailID, int FeeScheduleID, int StudentID, int ReceiptID, DataHelper dh, SqlTransaction trans);
        List<Batch> GetBatches(int ProductID, int CenterID, DateTime AfterDate);
        List<StudentSubscription> GetStudentSubscriptionDues(DateTime DueDate);
        int ValidateSubscriptionPayment(int SubscriptionID, decimal PaidAmount, decimal PaidTax, DateTime DueDate,
                                                    int FeeSchID, string TransactionNo = null);
        int SaveSubscriptionTransactionDetails(StudentSubscriptionTransaction payment);
        StudentSubscriptionTransaction GetSubscriptionTransactionDetails(string TransactionNo, int FeeScheduleID = 0);
        void UpdateSubscriptionTransactionDetails(int SubscriptionTransactionDetailID, int ReceiptID, string TransactionStatus, DataHelper dh, SqlTransaction trans);
        RegistrationCourseEnrolment GetRegistrationCourseEnrolment(string CustomerID);
        PrintFeeSchedule GetPrintFeeScheduleDetails(int FeeScheduleID);
        List<string> GetReceiptInvoices(int ReceiptDetailID);
        PrintTaxInvoice GetPrintTaxInvoiceDetails(string InvoiceNo, int ReceiptDetailID);
        int SaveNewSubscription(string TransactionNo, int PlanID, int ProductID, string SubscriptionPlanID, string AuthKey, int BillingPeriod,
                                        DateTime BillingStartDate, DateTime BillingEndDate, decimal BillingAmount, string SubscriptionStatus,
                                        string MandateLink);
        void UpdateTransactionStatus(string TransactionNo, string TransactionStatus);
        void SaveLogs(RequestLog log);
        StudentPayOut GetStudentPayoutDues(string CustomerID);
        int ValidatePayOutPayment(int FeeScheduleID, decimal PaidAmount, decimal PaidTax);
        int SavePayoutTransactionDetails(StudentPayOutTransaction payment, int FeeScheduleID);
        List<StudentPayOutTransaction> GetPayoutTransactionDetails(string TransactionNo, int FeeScheduleID = 0);
        void UpdatePayoutTransactionDetails(int PayoutTransactionID, int ReceiptID, string TransactionStatus, DataHelper dh, SqlTransaction trans);
        Registration GetRegistrationDetails(string CustomerID = null, int RegID = 0);
        List<SocialCategory> GetSocialCategoryList();
        List<ProductSubscription> GetCustomerMandateLinks(string CustomerID);
        int SaveProduct(Product product, string CreatedBy);
        void SaveProductExamCategoriesMappings(int ProductID, string ExamCategoryIDList, string CreatedBy, SqlTransaction trans, DataHelper dh);
        void SaveProductCenterFeePlanMappings(int ProductID, int CenterID, int FeePlanID, DateTime ValidFrom, DateTime ValidTo, string CreatedBy, SqlTransaction trans, DataHelper dh);
        List<FeePlan> GetFeePlanList(int CenterID, int ProductID);
        List<Product> GetProductsForPublishing(int BrandID, int CenterID = 0, int CategoryID = 0);
        int ActivateDeActivateProduct(int ProductID, bool Activate);
        int SavePlan(Plan plan, string CreatedBy);
        int ActivateDeActivatePlan(int PlanID, bool Activate);
        PlanConfig SavePlanConfig(PlanConfig config);
        List<Plan> GetPlanListForPublishing(string BrandIDList, bool CanBePublished = true);
        void SavePlanProductMap(string ProductIDList, int PlanID);
        int ValidateUser(string LoginID, string Password);
        List<Plan> GetPlanListForCouponMapping(string BrandIDList, int CategoryID, string ExamCategoryIDs = null, int CouponID = 0);
        void UpdateEnquiryOnAdmission(int EnquiryID, SqlTransaction trans, DataHelper dh);
        List<Order> GetOrders(string CustomerID);
        void CloseExistingSameCourseAccounts(string CustomerID, int ProductID, SqlTransaction trans, DataHelper dh);
        void SetAdmissionFeeSchedule(int FeeScheduleID, SqlTransaction trans, DataHelper dh);
        int InsertStudentBatchInQueue(int StudentDetailID, int BatchID, string EntryType, SqlTransaction trans, DataHelper dh);
        FAQ GetFAQForPlan(int PlanID);
        void CalculateStudentStatus(string Status, int StudentDetailID);
        void ResetDataForTesting(string CustomerID);
        void SaveBatchNotification(string CustomerID, int PlanID, int ProductID, int CenterID, string MobileNo, string EmailID = "");
        void InsertStudentStatusInQueue();
        void InsertStudentLoginDetails(string LoginID, int UserID, SqlTransaction trans, DataHelper dh);
        AboutBrand GetAboutBrandDetails(string ToBeDisplayedIn, int BrandID = 109);
        List<Syllabus> GetSyllabusForProduct(int PlanID, int ProductID = 0);
        List<Schedule> GetScheduleForProduct(int PlanID, int ProductID = 0);
        List<ExamGroup> GetExamGroups(int BrandID);
        CampaignCoupon GenerateCouponCodeForCampaign(string CampaignName, decimal MarksObtained, string CustomerID,
                                                            DateTime? ValidFrom = null, DateTime? ValidTo = null,int LanguageID=0);
        List<Product> GetProductList(int BrandID, int CategoryID = 0, string ExamGroups = null, int PlanID = 0);
        List<CouponCard> GetCouponCards(string CustomerID, string CouponType, string CampaignName = null);
        List<IssueCategory> GetIssueCategories();
        int SaveCustomerIssue(string Issue, int IssueCategoryID, string Name, string StudentID, string CustomerID,
                                            string ContactNo, string EmailID);
        List<Coupon> GetCouponsPerCustomerPlan(int PlanID, String CustomerID, int ProductID, int centerID, int PaymentMode,double PurchaseAmount);

        List<Campaign> GetActiveCampaigns();
        List<RecommendedCourseProductPlan> GetRecommendedCourseLists(int CourseID);
        List<Product> GetRecommendedProductList(int CourseID);
        StudentRegDetails UpdateStudentProfile(Registration reg);
    }
}
