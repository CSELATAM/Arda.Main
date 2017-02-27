﻿using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Arda.Kanban.Models;
using Arda.Kanban.Repositories;
using Arda.Common.ViewModels.Main;

namespace IntegrationTests
{
    public class Workload : ISupportSnapshot<WorkloadViewModel>
    {
        public IEnumerable<WorkloadViewModel> GetSnapshot(TransactionalKanbanContext context)
        {
            WorkloadRepository workload = new WorkloadRepository(context);

            return workload.GetAllWorkloads().ToArray();
        }

        [Fact]
        public void Workload_GetAllWorkloads_Should_ReturnAllValues() 
        {
            ArdaTestMgr.Validate(this, $"Workload.GetAllWorkloads()",
                (list, ctx) => {
                    var rows = from r in list
                               select r.WBTitle;

                    return rows;
                });
        }

        [Fact]
        public void Workload_AddNewWorkload_Should_AddRow()
        {
            WorkloadViewModel WORKLOAD1 = new WorkloadViewModel()
            {
                WBActivity = Guid.Parse("1f265df5-adbe-4b7b-a05a-451af058c482"), // POC
                WBComplexity = 1,
                WBCreatedBy = "admin@ardademo.onmicrosoft.com",
                WBCreatedDate = DateTime.Parse("2021-01-20"),
                WBEndDate = DateTime.Parse("2021-01-30"),
                WBDescription = "My Workload Description",
                WBExpertise = 2,
                WBFilesList = null,
                WBID = Guid.Parse("aaaa0022-FD15-428C-9B24-14E6467977AD"),
                WBIsWorkload = true,
                WBMetrics = new Guid[] {
                    Guid.Parse("6da887cb-9edd-42cb-87c9-83ac772d9b65"), // Community
                    Guid.Parse("45979112-aff6-4bfa-878b-02baa8fd1074")  // Education
                },
                WBStartDate = DateTime.Parse("2021-01-25"),
                WBStatus = 3,
                WBTechnologies = new Guid[] {
                    Guid.Parse("9c263d44-2c11-48cd-b876-5ebb540bbf51"), // Infra
                    Guid.Parse("af5d8796-0ca2-4d54-84f7-d3194f5f2426")  // Web & Mobile
                },
                WBTitle = "My Initial Workload",
                WBUsers = new string[] {
                    "user@ardademo.onmicrosoft.com", "admin@ardademo.onmicrosoft.com"
                }
            };

            ArdaTestMgr.Validate(this, $"Workload.AddNewWorkload()",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);
                    
                    workload.AddNewWorkload(WORKLOAD1);

                    return workload.GetAllWorkloads();
                });
        }
        
        [Fact]
        public void Workload_DeleteWorkloadByID_Should_ReturnRemoveExactlyOne()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.DeleteWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);
                    
                    workload.DeleteWorkloadByID(Guid.Parse(GUID));

                    return workload.GetAllWorkloads();
                });
        }

        [Fact]
        public void Workload_GetWorkloadByID_Should_ReturnExactlyOne()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadByID({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = workload.GetWorkloadByID(Guid.Parse(GUID));

                    return row;
                });
        }

        [Fact]
        public void Workload_GetWorkloadsByUser_Should_ReturnUserData()
        {
            string USER_UNIQUENAME = "admin@ardademo.onmicrosoft.com";

            ArdaTestMgr.Validate(this, $"Workload.GetWorkloadsByUser({USER_UNIQUENAME})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var rows = workload.GetWorkloadsByUser(USER_UNIQUENAME);

                    return rows;
                });
        }

        [Fact]
        public void Workload_EditWorkload_Should_ChangeRow()
        {
            string GUID = "{...}";

            ArdaTestMgr.Validate(this, $"Workload.EditWorkload({GUID})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = (from r in list
                               where r.WBID == Guid.Parse(GUID)
                               select r).First();

                    // edit

                    workload.EditWorkload(row);

                    return workload.GetAllWorkloads();
                });
        }        

        [Fact]
        public void Workload_UpdateWorkloadStatus_Should_UpdateOneStatus()
        {
            string GUID = "{...}";
            int STATUS = 4;

            ArdaTestMgr.Validate(this, $"Workload.UpdateWorkloadStatus({GUID},{STATUS})",
                (list, ctx) => {
                    WorkloadRepository workload = new WorkloadRepository(ctx);

                    var row = workload.UpdateWorkloadStatus(Guid.Parse(GUID), STATUS);

                    return row;
                });
        }


    }
}
