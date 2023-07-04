USE [sistema_banco]
GO
/****** Object:  Table [dbo].[contas]    Script Date: 30/06/2023 16:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contas](
	[codConta] [varchar](25) NOT NULL,
	[agencia] [varchar](6) NOT NULL,
	[senha] [varchar](100) NULL,
	[saldo] [money] NOT NULL,
	[tipo] [tinyint] NOT NULL,
	[idUsuario] [int] NULL,
 CONSTRAINT [PK__contas__B89B358CE048E355] PRIMARY KEY CLUSTERED 
(
	[codConta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mov]    Script Date: 30/06/2023 16:22:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mov](
	[idMov] [int] IDENTITY(1,1) NOT NULL,
	[idConta] [varchar](25) NOT NULL,
	[dataHora] [datetime] NULL,
	[valor] [money] NOT NULL,
	[tipo] [varchar](20) NOT NULL,
 CONSTRAINT [PK__mov__3DC69A4F779BE56D] PRIMARY KEY CLUSTERED 
(
	[idMov] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuarios]    Script Date: 30/06/2023 16:22:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](100) NOT NULL,
	[email] [varchar](100) NULL,
	[telefone] [varchar](15) NULL,
	[cpf] [varchar](15) NULL,
 CONSTRAINT [PK__usuarios__3213E83F7722FE27] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[contas] ADD  CONSTRAINT [DF__contas__saldo__4BAC3F29]  DEFAULT ((0)) FOR [saldo]
GO
ALTER TABLE [dbo].[mov]  WITH CHECK ADD  CONSTRAINT [FK__mov__idConta__571DF1D5] FOREIGN KEY([idConta])
REFERENCES [dbo].[contas] ([codConta])
GO
ALTER TABLE [dbo].[mov] CHECK CONSTRAINT [FK__mov__idConta__571DF1D5]
GO
