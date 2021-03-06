USE [master]
GO
/****** Object:  Database [RMMessenger]    Script Date: 01-Aug-20 13:21:33 ******/
CREATE DATABASE [RMMessenger]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RMMessenger', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RMMessenger.mdf' , SIZE = 204800KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RMMessenger_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\RMMessenger_log.ldf' , SIZE = 204800KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RMMessenger] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RMMessenger].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RMMessenger] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RMMessenger] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RMMessenger] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RMMessenger] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RMMessenger] SET ARITHABORT OFF 
GO
ALTER DATABASE [RMMessenger] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RMMessenger] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RMMessenger] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RMMessenger] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RMMessenger] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RMMessenger] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RMMessenger] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RMMessenger] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RMMessenger] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RMMessenger] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RMMessenger] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RMMessenger] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RMMessenger] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RMMessenger] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RMMessenger] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RMMessenger] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RMMessenger] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RMMessenger] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RMMessenger] SET  MULTI_USER 
GO
ALTER DATABASE [RMMessenger] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RMMessenger] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RMMessenger] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RMMessenger] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RMMessenger] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RMMessenger] SET QUERY_STORE = OFF
GO
USE [RMMessenger]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 01-Aug-20 13:21:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[User_Account_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](50) NOT NULL,
	[First_Name] [nvarchar](50) NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Profile_Picture] [varbinary](max) NULL,
	[Status] [nvarchar](100) NULL,
	[Email] [nvarchar](50) NULL,
	[PostalCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_User_Account] PRIMARY KEY CLUSTERED 
(
	[User_Account_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddRequests]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddRequests](
	[AddRequest_ID] [int] IDENTITY(1,1) NOT NULL,
	[SentBy_User_ID] [nvarchar](50) NOT NULL,
	[SentTo_User_ID] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Date] [date] NULL,
 CONSTRAINT [PK_AddRequests] PRIMARY KEY CLUSTERED 
(
	[AddRequest_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AddressBook]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressBook](
	[AddressBook_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](50) NOT NULL,
	[Friend_User_ID] [nvarchar](50) NOT NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_AddressBook] PRIMARY KEY CLUSTERED 
(
	[AddressBook_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailConfirmation]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailConfirmation](
	[EmailConfirmation_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](50) NOT NULL,
	[Code] [int] NOT NULL,
	[IsConfirmed] [bit] NULL,
 CONSTRAINT [PK_EmailConfirmation] PRIMARY KEY CLUSTERED 
(
	[EmailConfirmation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friendships]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friendships](
	[Friendship_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](50) NOT NULL,
	[Friend_ID] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Friendships] PRIMARY KEY CLUSTERED 
(
	[Friendship_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginSessions]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginSessions](
	[LoginSession_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_LoginSessions] PRIMARY KEY CLUSTERED 
(
	[LoginSession_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[Message_ID] [int] IDENTITY(1,1) NOT NULL,
	[SentBy_User_ID] [nvarchar](50) NOT NULL,
	[SentTo_User_ID] [nvarchar](50) NOT NULL,
	[Text] [nvarchar](max) NULL,
	[FontStyle] [nvarchar](50) NULL,
	[FontSize] [nvarchar](50) NULL,
	[Bold] [bit] NULL,
	[Italic] [bit] NULL,
	[Underline] [bit] NULL,
	[Color] [nvarchar](50) NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK__Messages__99FC143B2D86C59C] PRIMARY KEY CLUSTERED 
(
	[Message_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecentList]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecentList](
	[RecentList_ID] [int] IDENTITY(1,1) NOT NULL,
	[Sent_By] [nvarchar](50) NOT NULL,
	[Sent_To] [nvarchar](50) NOT NULL,
	[Date] [date] NULL,
 CONSTRAINT [PK_RecentContacts] PRIMARY KEY CLUSTERED 
(
	[RecentList_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Uploads]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Uploads](
	[Upload_ID] [int] IDENTITY(1,1) NOT NULL,
	[SentBy_User_ID] [nvarchar](50) NOT NULL,
	[SentTo_User_ID] [nvarchar](50) NOT NULL,
	[Uploaded_File] [varbinary](max) NOT NULL,
	[File_Name] [nvarchar](100) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Uploads] PRIMARY KEY CLUSTERED 
(
	[Upload_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01-Aug-20 13:21:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_ID] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Unique_User_ID]    Script Date: 01-Aug-20 13:21:34 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Unique_User_ID] ON [dbo].[Accounts]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Users]
GO
ALTER TABLE [dbo].[AddRequests]  WITH CHECK ADD  CONSTRAINT [FK_AddRequests_SentBy] FOREIGN KEY([SentBy_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[AddRequests] CHECK CONSTRAINT [FK_AddRequests_SentBy]
GO
ALTER TABLE [dbo].[AddRequests]  WITH CHECK ADD  CONSTRAINT [FK_AddRequests_SentTo] FOREIGN KEY([SentTo_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[AddRequests] CHECK CONSTRAINT [FK_AddRequests_SentTo]
GO
ALTER TABLE [dbo].[AddressBook]  WITH CHECK ADD  CONSTRAINT [FK_AddressBook_Friend_User] FOREIGN KEY([Friend_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[AddressBook] CHECK CONSTRAINT [FK_AddressBook_Friend_User]
GO
ALTER TABLE [dbo].[AddressBook]  WITH CHECK ADD  CONSTRAINT [FK_AddressBook_User] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[AddressBook] CHECK CONSTRAINT [FK_AddressBook_User]
GO
ALTER TABLE [dbo].[Friendships]  WITH CHECK ADD  CONSTRAINT [FK_Friendships_Friend_ID] FOREIGN KEY([Friend_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Friendships] CHECK CONSTRAINT [FK_Friendships_Friend_ID]
GO
ALTER TABLE [dbo].[Friendships]  WITH CHECK ADD  CONSTRAINT [FK_Friendships_User_ID] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Friendships] CHECK CONSTRAINT [FK_Friendships_User_ID]
GO
ALTER TABLE [dbo].[LoginSessions]  WITH CHECK ADD  CONSTRAINT [FK_LoginSessions_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[LoginSessions] CHECK CONSTRAINT [FK_LoginSessions_Users]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Receiver_User] FOREIGN KEY([SentBy_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Receiver_User]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Sender_User] FOREIGN KEY([SentTo_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Sender_User]
GO
ALTER TABLE [dbo].[RecentList]  WITH CHECK ADD  CONSTRAINT [FK_RecentList_Sent_By] FOREIGN KEY([Sent_By])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[RecentList] CHECK CONSTRAINT [FK_RecentList_Sent_By]
GO
ALTER TABLE [dbo].[RecentList]  WITH CHECK ADD  CONSTRAINT [FK_RecentList_Sent_To] FOREIGN KEY([Sent_To])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[RecentList] CHECK CONSTRAINT [FK_RecentList_Sent_To]
GO
ALTER TABLE [dbo].[Uploads]  WITH CHECK ADD  CONSTRAINT [FK_Uploads_SentBy_User_ID] FOREIGN KEY([SentBy_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Uploads] CHECK CONSTRAINT [FK_Uploads_SentBy_User_ID]
GO
ALTER TABLE [dbo].[Uploads]  WITH CHECK ADD  CONSTRAINT [FK_Uploads_SentTo_User_ID] FOREIGN KEY([SentTo_User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
ALTER TABLE [dbo].[Uploads] CHECK CONSTRAINT [FK_Uploads_SentTo_User_ID]
GO
USE [master]
GO
ALTER DATABASE [RMMessenger] SET  READ_WRITE 
GO
