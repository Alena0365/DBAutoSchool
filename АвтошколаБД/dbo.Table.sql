CREATE TABLE [dbo].[t_cities]
(
[f_id] INT IDENTITY (1, 1) NOT NULL,
[f_name] NVARCHAR (50) NOT NULL,
PRIMARY KEY CLUSTERED ([f_id] ASC),
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [idx_cities_name]
ON [dbo].[t_cities]([f_name] ASC);

