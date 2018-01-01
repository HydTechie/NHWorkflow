using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Core
{
    public partial class Member : BaseEntity
    {
         
        //        [MemberId] //        [uniqueidentifier]
        //        NOT NULL,
        public Guid MemberId { get; set; }

        //[FirstName] [nvarchar] (50) NOT NULL,
        public string FirstName { get; set; }
        // [LastName] [nvarchar] (50) NOT NULL,
        public string LastName { get; set; }
        //  [DOB] [smalldatetime]     //        NOT NULL,
        public string DOB { get; set; }


        //  [Gender] [char](3) NULL,
        public string Gender { get; set; }
        //	[HearingAid] [char](3) NULL,
        public string HearingAid { get; set; }
        //	[Email] [nvarchar] (50) NULL
        public string Email { get; set; }
    }


    public partial class ApprovalQueue : BaseEntity
    {
        //       [ApprovalId]
        //       [uniqueidentifier]
        //       NOT NULL,
        public Guid ApprovalId { get; set; }

        //      [MemberId] [uniqueidentifier]
        //       NOT NULL,
        public Guid MemberId { get; set; }
        //      [CategoryId] [uniqueidentifier]
        //       NOT NULL,
        public Guid CategoryId { get; set; }

        //      [TotalCost] [money] NULL,
        public decimal? TotalCost { get; set; }
        //[Discount] [money] NULL,
        public decimal? Discount { get; set; }
        //[NetCost] [money] NULL,
        public decimal? NetCost { get; set; }

        //[ApprovalStatus] [bit] NULL,
        public bool? ApprovalStatus { get; set; }
        //[ApprovedBy] [uniqueidentifier] NULL,
        public string ApprovedBy { get; set; }
        //[LastModifiedDate] [datetime] NULL,
        public DateTimeOffset LastModifiedDate { get; set; }
        //[LastModifiedBy] [uniqueidentifier] NULL
        public string LastModifiedBy { get; set; }
    }

    /*
    
     * */
    public class BenefitCategory: BaseEntity
    {
        //        [CategoryId]
        //        [uniqueidentifier]
        //        NOT NULL,
        public Guid CategoryId { get; set; }
        //[CategoryType] [char](1) NULL,
        public string CategoryType { get; set; }
        //	[FederalDiscountPercentage] [decimal] NULL,
        public decimal FederalDiscountPercentage { get; set; }
        //	[MfgDiscountPercentage] [decimal] NULL
        public decimal MfgDiscountPercentage { get; set; }
    }


    public class Login : BaseEntity
    {

        //       [LoginId]
        //       [uniqueidentifier]
        //       NOT NULL,
        public Guid LoginId { get; set; }

        //   [Username] [nvarchar] (50) NULL,
        public string Username { get; set; }
        //[PasswordHash] [nvarchar] (500) NULL,
        public string PasswordHash { get; set; }
        //[Email] [nchar] (10) NULL
        public string Email { get; set; }
    }

}
