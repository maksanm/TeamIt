using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImageId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Picture_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Picture",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PictureId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_AspNetUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Team_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JoinTeamRequest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    UserToAddId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestSenderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinTeamRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinTeamRequest_AspNetUsers_RequestSenderId",
                        column: x => x.RequestSenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinTeamRequest_AspNetUsers_UserToAddId",
                        column: x => x.UserToAddId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JoinTeamRequest_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UseOwnHierarchy = table.Column<bool>(type: "bit", nullable: false),
                    CreatorTeamId = table.Column<long>(type: "bigint", nullable: false),
                    LimitRoleId = table.Column<long>(type: "bigint", nullable: false),
                    PictureId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Project_Role_LimitRoleId",
                        column: x => x.LimitRoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Team_CreatorTeamId",
                        column: x => x.CreatorTeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleTeam",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTeam", x => new { x.RoleId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_RoleTeam_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamProfile_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamProfile_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamProfile_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChatPictureId = table.Column<long>(type: "bigint", nullable: true),
                    BaseTeamId = table.Column<long>(type: "bigint", nullable: true),
                    BaseProjectId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_Picture_ChatPictureId",
                        column: x => x.ChatPictureId,
                        principalTable: "Picture",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chat_Project_BaseProjectId",
                        column: x => x.BaseProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chat_Team_BaseTeamId",
                        column: x => x.BaseTeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProfile_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectProfile_TeamProfile_TeamProfileId",
                        column: x => x.TeamProfileId,
                        principalTable: "TeamProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    TeamProfileId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatProfile_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatProfile_ProjectProfile_ProjectProfileId",
                        column: x => x.ProjectProfileId,
                        principalTable: "ProjectProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatProfile_TeamProfile_TeamProfileId",
                        column: x => x.TeamProfileId,
                        principalTable: "TeamProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeadLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    AssigneeProfileId = table.Column<long>(type: "bigint", nullable: true),
                    ParentTaskId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_ProjectProfile_AssigneeProfileId",
                        column: x => x.AssigneeProfileId,
                        principalTable: "ProjectProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Task_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Task_Task_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "Task",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    AttachedImageId = table.Column<long>(type: "bigint", nullable: true),
                    SenderProfileId = table.Column<long>(type: "bigint", nullable: true),
                    ChatProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_ChatProfile_ChatProfileId",
                        column: x => x.ChatProfileId,
                        principalTable: "ChatProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_ChatProfile_SenderProfileId",
                        column: x => x.SenderProfileId,
                        principalTable: "ChatProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Picture_AttachedImageId",
                        column: x => x.AttachedImageId,
                        principalTable: "Picture",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Edit team preferences" },
                    { 2L, "Delete team" },
                    { 3L, "Add new user to the team" },
                    { 4L, "Kick user from the team" },
                    { 5L, "Assign role to user" },
                    { 6L, "Create, delete and set up role" },
                    { 7L, "Create project for team" },
                    { 8L, "Add team to other teams projects" },
                    { 9L, "Leave other team project with the whole team" },
                    { 10L, "Delete project created by team" },
                    { 11L, "Edit project preferences" },
                    { 12L, "Add new teams to the project" },
                    { 13L, "Kick team from the project" },
                    { 14L, "Set role that restricts third party teams permissions" },
                    { 15L, "Create new tasks" },
                    { 16L, "Assign user to complete the task" },
                    { 17L, "Create subtasks for existing tasks" },
                    { 18L, "Change task state, description, start date and deadline" },
                    { 19L, "Delete task with all subtasks" },
                    { 20L, "Change chat name and picture" },
                    { 21L, "Delete chat" },
                    { 22L, "Add user from base team or project to the chat" },
                    { 23L, "Send image to the chat" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Team creator" },
                    { 2L, "Guest" }
                });

            migrationBuilder.InsertData(
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 2L, 1L },
                    { 3L, 1L },
                    { 4L, 1L },
                    { 5L, 1L },
                    { 6L, 1L },
                    { 7L, 1L },
                    { 8L, 1L },
                    { 9L, 1L },
                    { 10L, 1L },
                    { 11L, 1L },
                    { 12L, 1L },
                    { 13L, 1L },
                    { 14L, 1L },
                    { 15L, 1L },
                    { 16L, 1L },
                    { 17L, 1L },
                    { 18L, 1L },
                    { 19L, 1L },
                    { 20L, 1L },
                    { 21L, 1L },
                    { 22L, 1L },
                    { 23L, 1L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_BaseProjectId",
                table: "Chat",
                column: "BaseProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_BaseTeamId",
                table: "Chat",
                column: "BaseTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ChatPictureId",
                table: "Chat",
                column: "ChatPictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatProfile_ChatId",
                table: "ChatProfile",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatProfile_ProjectProfileId",
                table: "ChatProfile",
                column: "ProjectProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatProfile_TeamProfileId",
                table: "ChatProfile",
                column: "TeamProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinTeamRequest_RequestSenderId",
                table: "JoinTeamRequest",
                column: "RequestSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinTeamRequest_TeamId",
                table: "JoinTeamRequest",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinTeamRequest_UserToAddId",
                table: "JoinTeamRequest",
                column: "UserToAddId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_AttachedImageId",
                table: "Message",
                column: "AttachedImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatId",
                table: "Message",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChatProfileId",
                table: "Message",
                column: "ChatProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderProfileId",
                table: "Message",
                column: "SenderProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CreatorTeamId",
                table: "Project",
                column: "CreatorTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_LimitRoleId",
                table: "Project",
                column: "LimitRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_PictureId",
                table: "Project",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProfile_ProjectId",
                table: "ProjectProfile",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProfile_TeamProfileId",
                table: "ProjectProfile",
                column: "TeamProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleTeam_TeamId",
                table: "RoleTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_AssigneeProfileId",
                table: "Task",
                column: "AssigneeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ParentTaskId",
                table: "Task",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_ProjectId",
                table: "Task",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_CreatorUserId",
                table: "Team",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_PictureId",
                table: "Team",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamProfile_RoleId",
                table: "TeamProfile",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamProfile_TeamId",
                table: "TeamProfile",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamProfile_UserId",
                table: "TeamProfile",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "JoinTeamRequest");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "RoleTeam");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ChatProfile");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropTable(
                name: "ProjectProfile");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "TeamProfile");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Picture");
        }
    }
}
