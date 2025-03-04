USE [CoworkingSpaceDB]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 04/01/2025 20:57:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[address_id] [int] IDENTITY(1,1) NOT NULL,
	[street] [nvarchar](100) NULL,
	[house_number] [nvarchar](10) NULL,
	[postal_code] [nvarchar](10) NULL,
	[city] [nvarchar](50) NULL,
	[state] [nvarchar](50) NULL,
	[country] [nvarchar](50) NULL,
 CONSTRAINT [PK__Address__CAA247C8E6D97059] PRIMARY KEY CLUSTERED 
(
	[address_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddressType]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressType](
	[address_type_id] [int] IDENTITY(1,1) NOT NULL,
	[address_type] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
 CONSTRAINT [PK__AddressT__5ABF11E50EED6709] PRIMARY KEY CLUSTERED 
(
	[address_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[MiddleName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Prefix] [nvarchar](max) NULL,
	[Suffix] [nvarchar](max) NULL,
	[Nickname] [nvarchar](max) NULL,
	[RecoveryEmail] [nvarchar](max) NULL,
	[AlternaiveEmail] [nvarchar](max) NULL,
	[RecoveryPhoneNumber] [nvarchar](max) NULL,
	[Gender] [nvarchar](max) NULL,
	[Birthday] [date] NULL,
	[ProfilePicturePath] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[JobTitle] [nvarchar](max) NULL,
	[Department] [nvarchar](max) NULL,
	[AppLanguage] [nvarchar](max) NULL,
	[Website] [nvarchar](max) NULL,
	[Linkedin] [nvarchar](max) NULL,
	[Facebook] [nvarchar](max) NULL,
	[Instagram] [nvarchar](max) NULL,
	[Twitter] [nvarchar](max) NULL,
	[Github] [nvarchar](max) NULL,
	[Youtube] [nvarchar](max) NULL,
	[Tiktok] [nvarchar](max) NULL,
	[Snapchat] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[LastLogin] [datetime2](7) NULL,
	[Status] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [nvarchar](450) NOT NULL,
	[desk_id] [int] NULL,
	[start_time] [datetime] NOT NULL,
	[end_time] [datetime] NOT NULL,
	[is_cancelled] [bit] NOT NULL,
	[cancellation_reason] [nvarchar](255) NULL,
	[is_checked_in] [bit] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
	[RoomId] [int] NULL,
	[total_cost] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK__Booking__5DE3A5B11355DBAA] PRIMARY KEY CLUSTERED 
(
	[booking_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[company_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[industry] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[registration_number] [nvarchar](50) NULL,
	[tax_id] [nvarchar](50) NULL,
	[website] [nvarchar](255) NULL,
	[contact_email] [nvarchar](100) NULL,
	[contact_phone] [nvarchar](20) NULL,
	[founded_date] [date] NULL,
 CONSTRAINT [PK__Company__3E267235DB003AF5] PRIMARY KEY CLUSTERED 
(
	[company_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyAddress]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyAddress](
	[company_address_id] [int] IDENTITY(1,1) NOT NULL,
	[company_id] [int] NULL,
	[address_id] [int] NULL,
	[address_type_id] [int] NULL,
	[is_default] [bit] NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
 CONSTRAINT [PK__CompanyA__5650FC57FCFDFEB6] PRIMARY KEY CLUSTERED 
(
	[company_address_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyCEO]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyCEO](
	[company_id] [int] NOT NULL,
	[ceo_user_id] [nvarchar](450) NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NULL,
 CONSTRAINT [PK__CompanyC__D8C45EEBF1ACE5BA] PRIMARY KEY CLUSTERED 
(
	[company_id] ASC,
	[ceo_user_id] ASC,
	[start_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyEmployee]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyEmployee](
	[company_id] [int] NOT NULL,
	[user_id] [nvarchar](450) NOT NULL,
	[start_date] [date] NOT NULL,
	[position] [nvarchar](100) NULL,
	[end_date] [date] NULL,
 CONSTRAINT [PK__CompanyE__E51B67242C37EA90] PRIMARY KEY CLUSTERED 
(
	[company_id] ASC,
	[user_id] ASC,
	[start_date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Desk]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Desk](
	[desk_id] [int] IDENTITY(1,1) NOT NULL,
	[room_id] [int] NOT NULL,
	[desk_name] [nvarchar](100) NOT NULL,
	[price] [decimal](10, 2) NULL,
	[currency] [nvarchar](3) NULL,
	[is_available] [bit] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
 CONSTRAINT [PK__Desk__8AF10D0469E707D1] PRIMARY KEY CLUSTERED 
(
	[desk_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Label]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Label](
	[label_id] [int] IDENTITY(1,1) NOT NULL,
	[label_name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](255) NULL,
	[color_code] [nvarchar](7) NULL,
 CONSTRAINT [PK__Label__E44FFA585B07B173] PRIMARY KEY CLUSTERED 
(
	[label_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabelAssignment]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabelAssignment](
	[label_assignment_id] [int] IDENTITY(1,1) NOT NULL,
	[label_id] [int] NOT NULL,
	[entity_type] [nvarchar](50) NOT NULL,
	[entity_id] [int] NOT NULL,
 CONSTRAINT [PK__LabelAss__F27DEC24A29D32BB] PRIMARY KEY CLUSTERED 
(
	[label_assignment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[room_id] [int] IDENTITY(1,1) NOT NULL,
	[room_name] [nvarchar](100) NOT NULL,
	[room_type] [nvarchar](50) NULL,
	[price] [decimal](10, 2) NULL,
	[currency] [nvarchar](3) NULL,
	[is_active] [bit] NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
	[company_address_id] [int] NULL,
 CONSTRAINT [PK__Room__19675A8AFBEE5EFB] PRIMARY KEY CLUSTERED 
(
	[room_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAddress]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAddress](
	[user_address_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [nvarchar](450) NULL,
	[address_id] [int] NULL,
	[address_type_id] [int] NULL,
	[is_default] [bit] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
 CONSTRAINT [PK__UserAddr__FEC0352ECEF2EE95] PRIMARY KEY CLUSTERED 
(
	[user_address_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 04/01/2025 20:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSession](
	[session_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [nvarchar](450) NOT NULL,
	[login_time] [datetime] NULL,
	[logout_time] [datetime] NULL,
	[last_ip] [nvarchar](45) NULL,
	[device] [nvarchar](100) NULL,
	[browser] [nvarchar](50) NULL,
	[operating_system] [nvarchar](50) NULL,
	[is_active] [bit] NULL,
	[location] [nvarchar](100) NULL,
	[login_attempts] [int] NULL,
	[failed_login_attempts] [int] NULL,
	[session_duration]  AS (datediff(minute,[login_time],[logout_time])),
 CONSTRAINT [PK__UserSess__69B13FDCAFEF904D] PRIMARY KEY CLUSTERED 
(
	[session_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Booking] ADD  DEFAULT ((0.0)) FOR [total_cost]
GO
ALTER TABLE [dbo].[CompanyAddress] ADD  DEFAULT (CONVERT([bit],(0))) FOR [is_default]
GO
ALTER TABLE [dbo].[Desk] ADD  DEFAULT ((0.0)) FOR [price]
GO
ALTER TABLE [dbo].[Desk] ADD  DEFAULT (N'EUR') FOR [currency]
GO
ALTER TABLE [dbo].[Desk] ADD  DEFAULT (CONVERT([bit],(1))) FOR [is_available]
GO
ALTER TABLE [dbo].[Room] ADD  DEFAULT ((0.0)) FOR [price]
GO
ALTER TABLE [dbo].[Room] ADD  DEFAULT (N'EUR') FOR [currency]
GO
ALTER TABLE [dbo].[Room] ADD  DEFAULT (CONVERT([bit],(1))) FOR [is_active]
GO
ALTER TABLE [dbo].[UserAddress] ADD  DEFAULT (CONVERT([bit],(0))) FOR [is_default]
GO
ALTER TABLE [dbo].[UserSession] ADD  DEFAULT (getdate()) FOR [login_time]
GO
ALTER TABLE [dbo].[UserSession] ADD  DEFAULT (CONVERT([bit],(1))) FOR [is_active]
GO
ALTER TABLE [dbo].[UserSession] ADD  DEFAULT ((1)) FOR [login_attempts]
GO
ALTER TABLE [dbo].[UserSession] ADD  DEFAULT ((0)) FOR [failed_login_attempts]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__desk_id__7A672E12] FOREIGN KEY([desk_id])
REFERENCES [dbo].[Desk] ([desk_id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__desk_id__7A672E12]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__room_id] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([room_id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__room_id]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__user_id__797309D9] FOREIGN KEY([user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__user_id__797309D9]
GO
ALTER TABLE [dbo].[CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK__CompanyAd__addre__693CA210] FOREIGN KEY([address_id])
REFERENCES [dbo].[Address] ([address_id])
GO
ALTER TABLE [dbo].[CompanyAddress] CHECK CONSTRAINT [FK__CompanyAd__addre__693CA210]
GO
ALTER TABLE [dbo].[CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK__CompanyAd__addre__6A30C649] FOREIGN KEY([address_type_id])
REFERENCES [dbo].[AddressType] ([address_type_id])
GO
ALTER TABLE [dbo].[CompanyAddress] CHECK CONSTRAINT [FK__CompanyAd__addre__6A30C649]
GO
ALTER TABLE [dbo].[CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK__CompanyAd__compa__68487DD7] FOREIGN KEY([company_id])
REFERENCES [dbo].[Company] ([company_id])
GO
ALTER TABLE [dbo].[CompanyAddress] CHECK CONSTRAINT [FK__CompanyAd__compa__68487DD7]
GO
ALTER TABLE [dbo].[CompanyCEO]  WITH CHECK ADD  CONSTRAINT [FK__CompanyCE__ceo_u__6477ECF3] FOREIGN KEY([ceo_user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[CompanyCEO] CHECK CONSTRAINT [FK__CompanyCE__ceo_u__6477ECF3]
GO
ALTER TABLE [dbo].[CompanyCEO]  WITH CHECK ADD  CONSTRAINT [FK__CompanyCE__compa__6383C8BA] FOREIGN KEY([company_id])
REFERENCES [dbo].[Company] ([company_id])
GO
ALTER TABLE [dbo].[CompanyCEO] CHECK CONSTRAINT [FK__CompanyCE__compa__6383C8BA]
GO
ALTER TABLE [dbo].[CompanyEmployee]  WITH CHECK ADD  CONSTRAINT [FK__CompanyEm__compa__5FB337D6] FOREIGN KEY([company_id])
REFERENCES [dbo].[Company] ([company_id])
GO
ALTER TABLE [dbo].[CompanyEmployee] CHECK CONSTRAINT [FK__CompanyEm__compa__5FB337D6]
GO
ALTER TABLE [dbo].[CompanyEmployee]  WITH CHECK ADD  CONSTRAINT [FK__CompanyEm__user___60A75C0F] FOREIGN KEY([user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[CompanyEmployee] CHECK CONSTRAINT [FK__CompanyEm__user___60A75C0F]
GO
ALTER TABLE [dbo].[Desk]  WITH CHECK ADD  CONSTRAINT [FK__Desk__room_id__73BA3083] FOREIGN KEY([room_id])
REFERENCES [dbo].[Room] ([room_id])
GO
ALTER TABLE [dbo].[Desk] CHECK CONSTRAINT [FK__Desk__room_id__73BA3083]
GO
ALTER TABLE [dbo].[LabelAssignment]  WITH CHECK ADD  CONSTRAINT [FK__LabelAssi__label__3F466844] FOREIGN KEY([label_id])
REFERENCES [dbo].[Label] ([label_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabelAssignment] CHECK CONSTRAINT [FK__LabelAssi__label__3F466844]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK__Room__company_ad__70DDC3D8] FOREIGN KEY([company_address_id])
REFERENCES [dbo].[CompanyAddress] ([company_address_id])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK__Room__company_ad__70DDC3D8]
GO
ALTER TABLE [dbo].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK__UserAddre__addre__5165187F] FOREIGN KEY([address_id])
REFERENCES [dbo].[Address] ([address_id])
GO
ALTER TABLE [dbo].[UserAddress] CHECK CONSTRAINT [FK__UserAddre__addre__5165187F]
GO
ALTER TABLE [dbo].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK__UserAddre__addre__52593CB8] FOREIGN KEY([address_type_id])
REFERENCES [dbo].[AddressType] ([address_type_id])
GO
ALTER TABLE [dbo].[UserAddress] CHECK CONSTRAINT [FK__UserAddre__addre__52593CB8]
GO
ALTER TABLE [dbo].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK__UserAddre__user___5070F446] FOREIGN KEY([user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserAddress] CHECK CONSTRAINT [FK__UserAddre__user___5070F446]
GO
ALTER TABLE [dbo].[UserSession]  WITH CHECK ADD  CONSTRAINT [FK__UserSessi__user___59063A47] FOREIGN KEY([user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserSession] CHECK CONSTRAINT [FK__UserSessi__user___59063A47]
GO
