USE [master]
GO
/****** Object:  Database [dbForms]    Script Date: 22-03-2021 12:06:26 ******/
CREATE DATABASE [dbForms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbForms', FILENAME = N'D:\Databases2017\dbForms.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbForms_log', FILENAME = N'e:\Databases2017\dbForms_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [dbForms] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbForms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbForms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dbForms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dbForms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dbForms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dbForms] SET ARITHABORT OFF 
GO
ALTER DATABASE [dbForms] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dbForms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dbForms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dbForms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dbForms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dbForms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dbForms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dbForms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dbForms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dbForms] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dbForms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dbForms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dbForms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dbForms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dbForms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dbForms] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dbForms] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dbForms] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [dbForms] SET  MULTI_USER 
GO
ALTER DATABASE [dbForms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dbForms] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dbForms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dbForms] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dbForms] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [dbForms] SET QUERY_STORE = OFF
GO
USE [dbForms]
GO
/****** Object:  User [tpssDotNet]    Script Date: 22-03-2021 12:06:26 ******/
CREATE USER [tpssDotNet] FOR LOGIN [tpssDotNet] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [tpssDotNet]
GO
/****** Object:  Table [dbo].[DataForm]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataForm](
	[Id] [uniqueidentifier] NOT NULL,
	[FormName] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_DataForm] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataFormField]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFormField](
	[Id] [uniqueidentifier] NOT NULL,
	[FieldName] [nvarchar](150) NOT NULL,
	[FieldDisplayLabel] [nvarchar](150) NOT NULL,
	[FieldXML] [text] NULL,
	[DataFormFieldTypeId] [uniqueidentifier] NOT NULL,
	[DataFormSectionId] [uniqueidentifier] NOT NULL,
	[AttributeCheckedValue] [nvarchar](50) NULL,
	[AttributeDisabledValue] [nvarchar](50) NULL,
	[AttributeMaxValue] [nvarchar](50) NULL,
	[AttributeMaxlengthValue] [nvarchar](50) NULL,
	[AttributeMinValue] [nvarchar](50) NULL,
	[AttributePatternValue] [nvarchar](50) NULL,
	[AttributeReadonlyValue] [nvarchar](50) NULL,
	[AttributeRequiredValue] [nvarchar](50) NULL,
	[AttributeSizeValue] [nvarchar](50) NULL,
	[AttributeStepValue] [nvarchar](50) NULL,
	[AttributeValueValue] [nvarchar](50) NULL,
 CONSTRAINT [PK_DataFormField] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataFormFieldType]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFormFieldType](
	[Id] [uniqueidentifier] NOT NULL,
	[FieldTypeName] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL,
	[AttributeChecked] [bit] NOT NULL,
	[AttributeDisabled] [bit] NOT NULL,
	[AttributeMax] [bit] NOT NULL,
	[AttributeMaxlength] [bit] NOT NULL,
	[AttributeMin] [bit] NOT NULL,
	[AttributePattern] [bit] NOT NULL,
	[AttributeReadonly] [bit] NOT NULL,
	[AttributeRequired] [bit] NOT NULL,
	[AttributeSize] [bit] NOT NULL,
	[AttributeStep] [bit] NOT NULL,
	[AttributeValue] [bit] NOT NULL,
 CONSTRAINT [PK_DataFormFieldType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataFormSection]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFormSection](
	[Id] [uniqueidentifier] NOT NULL,
	[SectionName] [nvarchar](150) NOT NULL,
	[DataFormId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DataFormSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataFormSubmission]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFormSubmission](
	[Id] [uniqueidentifier] NOT NULL,
	[DataFormId] [uniqueidentifier] NOT NULL,
	[SubmitDateUTC] [datetime] NULL,
	[SubmitDateLocal] [datetime] NULL,
 CONSTRAINT [PK_DataFormSubmission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataFormSubmissionItem]    Script Date: 22-03-2021 12:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataFormSubmissionItem](
	[DataFormFieldId] [uniqueidentifier] NOT NULL,
	[DataFormSubmissionId] [uniqueidentifier] NOT NULL,
	[FieldIntValue] [int] NULL,
	[FieldStringValue] [nvarchar](150) NULL,
	[FieldDateValue] [datetime] NULL,
	[FieldFloatValue] [float] NULL,
	[FieldTextValue] [ntext] NULL,
	[FieldBinaryValue] [varbinary](max) NULL,
	[FieldGuidValue] [uniqueidentifier] NULL,
	[FieldBoolValue] [bit] NULL,
 CONSTRAINT [PK_DataFormSubmissionItem] PRIMARY KEY CLUSTERED 
(
	[DataFormFieldId] ASC,
	[DataFormSubmissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[DataForm] ([Id], [FormName]) VALUES (N'eab36f69-c86f-4bac-a1fd-dbaa65bd3e05', N'Form1')
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'92000659-4b9c-4002-9499-0e96af322e60', N'MyRange', N'Range', NULL, N'8caa9823-a76d-435e-a4ae-d97d890ae0fa', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'de5c3772-7d68-46f9-99e0-247657f544c7', N'MyDateTimeLocal', N'DateTimeLocal', NULL, N'a005b73b-eb0a-43e7-a5f4-ed207577dae8', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'18732fe3-8a81-4599-80c2-40f5c522c166', N'MyEmail', N'Email', NULL, N'a6acc779-6769-495e-bbc4-d95a320ef545', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'83bec035-5756-4893-95ba-79c76c78a09d', N'MyNumber', N'Number', NULL, N'ac90a0b4-e2c1-4020-8484-5dc2dbe2b80b', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'6a874b91-7234-4bac-88e4-7c85dbe747e6', N'MyCheckBox', N'CheckBox', NULL, N'f1d85542-000a-4157-ae5e-4f27fcad682e', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'57193492-8123-4f18-91a0-8dca1e2d9693', N'MyColor', N'Color', NULL, N'9a94a4aa-60cc-4384-ab47-b706ccef29f8', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'd56057d0-3e86-4b18-9d8a-a649b339142a', N'MyRadio', N'Radio', N'<control attr1=''test control''>
	<itemList group = ''test item group''>
		  <item name = ''item 1'' selected = '''' >
				<text>my r text 1</text>
				<value>my r value 1</value>
		  </item>
		  <item name = ''item 2'' selected = ''1'' >
				<text>my r text2</text>
				<value>my r value2</value>
		  </item>
		  <item name = ''item 3'' selected = '''' >
				<text>my r text 3</text>
				<value>my r value 3</value>
		  </item>
	</itemList>
</control>', N'43ea8882-69a3-42fa-900c-819ddff34ce7', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'ead211a2-676d-409a-b1e4-cd3d024e578a', N'MyTextField', N'Text Field', NULL, N'6181516f-535f-43fe-9b42-4c2d47a81a78', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'fd5f589e-577b-4f29-83b2-ddc233f1460b', N'MySelectList', N'SelectList', N'<control attr1=''test control''>
	<itemList group = ''test item group''>
		  <item name = ''item 1'' selected = '''' >
				<text>my text 1</text>
				<value>my value 1</value>
		  </item>
		  <item name = ''item 2'' selected = ''1'' >
				<text>my text2</text>
				<value>my value2</value>
		  </item>
		  <item name = ''item 3'' selected = '''' >
				<text>my text 3</text>
				<value>my value 3</value>
		  </item>
	</itemList>
</control>', N'772a2576-5b7f-4a9b-90eb-de93ebb683f1', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormField] ([Id], [FieldName], [FieldDisplayLabel], [FieldXML], [DataFormFieldTypeId], [DataFormSectionId], [AttributeCheckedValue], [AttributeDisabledValue], [AttributeMaxValue], [AttributeMaxlengthValue], [AttributeMinValue], [AttributePatternValue], [AttributeReadonlyValue], [AttributeRequiredValue], [AttributeSizeValue], [AttributeStepValue], [AttributeValueValue]) VALUES (N'a04ce71d-aa88-4c8c-b69e-e83ed5d25297', N'MyDate', N'Date', NULL, N'192c3189-81f9-4c9e-bf4c-31a2d9e37a8f', N'163981f3-01b9-41e0-9888-aeb42b9efc38', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'89844dbc-45d1-49c6-9936-3057871477b9', N'Image', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'192c3189-81f9-4c9e-bf4c-31a2d9e37a8f', N'Date', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'6181516f-535f-43fe-9b42-4c2d47a81a78', N'Text', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'f1d85542-000a-4157-ae5e-4f27fcad682e', N'CheckBox', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'ac90a0b4-e2c1-4020-8484-5dc2dbe2b80b', N'Number', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'b2744be8-0f37-4588-9b7f-7e18609dbec3', N'File', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'43ea8882-69a3-42fa-900c-819ddff34ce7', N'Radio', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'73eac5bb-06ed-4dcb-88ae-9499ba643eb7', N'Password', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'0fed9452-fa22-4bf5-881a-977f9454ca1f', N'Time', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'8f4c334b-83f6-4ab9-aaf0-a4d23aff7e02', N'URL', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'9a94a4aa-60cc-4384-ab47-b706ccef29f8', N'Color', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'5a4d8cf8-7d02-46c1-bc04-ced833d34490', N'Hidden', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'a6acc779-6769-495e-bbc4-d95a320ef545', N'Email', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'8caa9823-a76d-435e-a4ae-d97d890ae0fa', N'Range', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'772a2576-5b7f-4a9b-90eb-de93ebb683f1', N'SelectList', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'a005b73b-eb0a-43e7-a5f4-ed207577dae8', N'DateTimeLocal', 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'8f638a2b-5e44-4200-b6e2-f1fe4a0a5a1a', N'CheckBoxList', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'17f89285-d919-40d2-9a91-f5574d199c8f', N'Month', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'859768af-ffa7-4593-bd0a-fbfbe04f7fbf', N'Tel', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormFieldType] ([Id], [FieldTypeName], [Active], [AttributeChecked], [AttributeDisabled], [AttributeMax], [AttributeMaxlength], [AttributeMin], [AttributePattern], [AttributeReadonly], [AttributeRequired], [AttributeSize], [AttributeStep], [AttributeValue]) VALUES (N'c5e75fbb-7203-4184-a402-ffada8d46ba4', N'Week', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[DataFormSection] ([Id], [SectionName], [DataFormId]) VALUES (N'163981f3-01b9-41e0-9888-aeb42b9efc38', N'Section 1', N'eab36f69-c86f-4bac-a1fd-dbaa65bd3e05')
GO
INSERT [dbo].[DataFormSubmission] ([Id], [DataFormId], [SubmitDateUTC], [SubmitDateLocal]) VALUES (N'68c52584-683a-4360-8f80-08d8bc46235b', N'eab36f69-c86f-4bac-a1fd-dbaa65bd3e05', CAST(N'2021-01-19T06:47:49.947' AS DateTime), NULL)
GO
INSERT [dbo].[DataFormSubmission] ([Id], [DataFormId], [SubmitDateUTC], [SubmitDateLocal]) VALUES (N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', N'eab36f69-c86f-4bac-a1fd-dbaa65bd3e05', CAST(N'2021-02-10T06:46:58.623' AS DateTime), NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'92000659-4b9c-4002-9499-0e96af322e60', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, NULL, NULL, 74, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'92000659-4b9c-4002-9499-0e96af322e60', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, NULL, NULL, 65, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'de5c3772-7d68-46f9-99e0-247657f544c7', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, NULL, CAST(N'2021-01-19T15:19:00.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'de5c3772-7d68-46f9-99e0-247657f544c7', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, NULL, CAST(N'2021-02-26T12:16:00.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'18732fe3-8a81-4599-80c2-40f5c522c166', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, N'test@test.com', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'18732fe3-8a81-4599-80c2-40f5c522c166', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, N'test@test.com', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'83bec035-5756-4893-95ba-79c76c78a09d', N'68c52584-683a-4360-8f80-08d8bc46235b', 5, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'83bec035-5756-4893-95ba-79c76c78a09d', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'6a874b91-7234-4bac-88e4-7c85dbe747e6', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'6a874b91-7234-4bac-88e4-7c85dbe747e6', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'57193492-8123-4f18-91a0-8dca1e2d9693', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, N'#b12525', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'57193492-8123-4f18-91a0-8dca1e2d9693', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, N'#7f1a1a', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'd56057d0-3e86-4b18-9d8a-a649b339142a', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, N'my r value 3', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'd56057d0-3e86-4b18-9d8a-a649b339142a', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, N'my r value2', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'ead211a2-676d-409a-b1e4-cd3d024e578a', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, N'test', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'ead211a2-676d-409a-b1e4-cd3d024e578a', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, N'test', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'fd5f589e-577b-4f29-83b2-ddc233f1460b', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, N'my value 3', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'fd5f589e-577b-4f29-83b2-ddc233f1460b', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, N'my value2', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'a04ce71d-aa88-4c8c-b69e-e83ed5d25297', N'68c52584-683a-4360-8f80-08d8bc46235b', NULL, NULL, CAST(N'2021-01-28T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[DataFormSubmissionItem] ([DataFormFieldId], [DataFormSubmissionId], [FieldIntValue], [FieldStringValue], [FieldDateValue], [FieldFloatValue], [FieldTextValue], [FieldBinaryValue], [FieldGuidValue], [FieldBoolValue]) VALUES (N'a04ce71d-aa88-4c8c-b69e-e83ed5d25297', N'3e46d942-2c71-45b7-63d6-08d8cd8faa51', NULL, NULL, CAST(N'2021-02-26T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, NULL)
GO
ALTER TABLE [dbo].[DataForm] ADD  CONSTRAINT [DF_DataForm_DataFormId]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DataFormField] ADD  CONSTRAINT [DF_DataFormField_DataFormFieldId]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DataFormFieldType] ADD  CONSTRAINT [DF_DataFormFieldType_DataFormFieldTypeId]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DataFormFieldType] ADD  CONSTRAINT [DF_DataFormFieldType_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[DataFormSection] ADD  CONSTRAINT [DF_DataFormSection_DataFormSectionId]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DataFormSubmission] ADD  CONSTRAINT [DF_DataFormSubmission_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DataFormField]  WITH CHECK ADD  CONSTRAINT [FK_DataFormField_DataFormFieldType] FOREIGN KEY([DataFormFieldTypeId])
REFERENCES [dbo].[DataFormFieldType] ([Id])
GO
ALTER TABLE [dbo].[DataFormField] CHECK CONSTRAINT [FK_DataFormField_DataFormFieldType]
GO
ALTER TABLE [dbo].[DataFormField]  WITH CHECK ADD  CONSTRAINT [FK_DataFormField_DataFormSection] FOREIGN KEY([DataFormSectionId])
REFERENCES [dbo].[DataFormSection] ([Id])
GO
ALTER TABLE [dbo].[DataFormField] CHECK CONSTRAINT [FK_DataFormField_DataFormSection]
GO
ALTER TABLE [dbo].[DataFormSection]  WITH CHECK ADD  CONSTRAINT [FK_DataFormSection_DataForm] FOREIGN KEY([DataFormId])
REFERENCES [dbo].[DataForm] ([Id])
GO
ALTER TABLE [dbo].[DataFormSection] CHECK CONSTRAINT [FK_DataFormSection_DataForm]
GO
ALTER TABLE [dbo].[DataFormSubmission]  WITH CHECK ADD  CONSTRAINT [FK_DataFormSubmission_DataForm] FOREIGN KEY([DataFormId])
REFERENCES [dbo].[DataForm] ([Id])
GO
ALTER TABLE [dbo].[DataFormSubmission] CHECK CONSTRAINT [FK_DataFormSubmission_DataForm]
GO
ALTER TABLE [dbo].[DataFormSubmissionItem]  WITH CHECK ADD  CONSTRAINT [FK_DataFormSubmissionItem_DataFormField] FOREIGN KEY([DataFormFieldId])
REFERENCES [dbo].[DataFormField] ([Id])
GO
ALTER TABLE [dbo].[DataFormSubmissionItem] CHECK CONSTRAINT [FK_DataFormSubmissionItem_DataFormField]
GO
ALTER TABLE [dbo].[DataFormSubmissionItem]  WITH CHECK ADD  CONSTRAINT [FK_DataFormSubmissionItem_DataFormSubmission] FOREIGN KEY([DataFormSubmissionId])
REFERENCES [dbo].[DataFormSubmission] ([Id])
GO
ALTER TABLE [dbo].[DataFormSubmissionItem] CHECK CONSTRAINT [FK_DataFormSubmissionItem_DataFormSubmission]
GO
USE [master]
GO
ALTER DATABASE [dbForms] SET  READ_WRITE 
GO
