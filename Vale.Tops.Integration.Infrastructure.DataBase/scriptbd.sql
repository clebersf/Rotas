USE [Route]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Consistency_I_Route_Damper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Consistency_I_Route_Damper]
(
	-- Add the parameters for the function here
	@RouteId int, @ReplacedId int
)
RETURNS int
AS
BEGIN
	declare @consistency int = 1
	SELECT top 1  @consistency =  CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END
	FROM            (SELECT @RouteId as Id
					--UNION
					--SELECT @ReplacedId as Id
					) AS ACTIVE INNER JOIN
					dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
					dbo.rRouteOxD AS ROXD ON ROXD.Id = IDROUTE.Id INNER JOIN
					dbo.OxD ON dbo.OxD.Id = ROXD.OxDId INNER JOIN
					dbo.rRouteSequence AS SEQ ON ACTIVE.Id = SEQ.RouteId INNER JOIN
					dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
					dbo.rRouteSequence AS SEQ_EQP_NEXT ON SEQ_EQP_NEXT.RouteId = SEQ.RouteId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 2 INNER JOIN
					dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
					dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
					dbo.Feeder AS CM ON CM.Id = MED_CON_L1.Id INNER JOIN
					dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
					Consistency AS CON ON CON.Id = MED_CON_L2.Id INNER JOIN
					dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
					dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
					dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
					dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
					dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
					dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
					dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id


	return ISNULL(@consistency,0)

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Consistency_I_Route_Feeder]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Consistency_I_Route_Feeder]
(
	-- Add the parameters for the function here
	@RouteId int, @ReplacedId int
)
RETURNS int
AS
BEGIN
	declare @consistency int = 1
	SELECT top 1  @consistency =  CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END
	FROM            (SELECT @RouteId as Id
					--UNION
					--SELECT @ReplacedId as Id
					) AS ACTIVE INNER JOIN
					dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
					dbo.rRouteOxD AS ROXD ON ROXD.Id = IDROUTE.Id INNER JOIN
					dbo.OxD ON dbo.OxD.Id = ROXD.OxDId INNER JOIN
					dbo.rRouteSequence AS SEQ ON ACTIVE.Id = SEQ.RouteId INNER JOIN
					dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
					dbo.rRouteSequence AS SEQ_EQP_NEXT ON SEQ_EQP_NEXT.RouteId = SEQ.RouteId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
					dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
					dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
					dbo.Feeder AS CM ON CM.Id = MED_CON_L1.Id INNER JOIN
					dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
					Consistency AS CON ON CON.Id = MED_CON_L2.Id INNER JOIN
					dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
					dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
					dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
					dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
					dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
					dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
					dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id


	return ISNULL(@consistency,0)

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Consistency_I_Route_Reversal]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Consistency_I_Route_Reversal]
(
	-- Add the parameters for the function here
	@RouteId int, @ReplacedId int
)
RETURNS int
AS
BEGIN
	declare @consistency int = 1
	SELECT top 1  @consistency =  CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END
	FROM            (SELECT @RouteId as Id
                          --UNION
                          --SELECT @ReplacedId as Id
						  ) AS ACTIVE INNER JOIN
                         dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
                         dbo.rRouteOxD AS ROXD ON ROXD.Id = IDROUTE.Id INNER JOIN
                         dbo.OxD ON dbo.OxD.Id = ROXD.OxDId INNER JOIN
                         dbo.rRouteSequence AS SEQ ON ACTIVE.Id = SEQ.RouteId INNER JOIN
                         dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
                         dbo.rRouteSequence AS SEQ_EQP_NEXT ON SEQ_EQP_NEXT.RouteId = SEQ.RouteId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
                         dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
                         dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
                         dbo.Reversal AS RV ON RV.Id = MED_CON_L1.Id INNER JOIN
                         dbo.Location AS RV_L2 ON RV_L2.ParentId = MED_CON_L1.Id INNER JOIN
                         dbo.Location AS RV_L3 ON RV_L3.ParentId = RV_L2.Id INNER JOIN
                         dbo.Reference AS REF_POS ON REF_POS.Id = RV_L3.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id INNER JOIN
                         dbo.Location AS RV_L4 ON RV_L4.ParentId = RV_L3.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = RV_L4.Id INNER JOIN
                         dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id

	return @consistency

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Consistency_I_Route_Rule]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Consistency_I_Route_Rule]
(
	-- Add the parameters for the function here
	@RouteId int
)
RETURNS varchar (500)
AS
BEGIN
	/****** Script for SelectTopNRows command from SSMS  ******/

	-- Declaração de variáveis
	declare @consitency varchar (500) = '1'

	---- PARA ROTAS NA FILA (Não se aplica a rotas substitutas)
	---- Verifica limite de mutilização de correias por rotas
	select 
	@consitency = case when COUNT(*)+max(isnull(Queue.Qnt,0)) <= (max(cast(ValueFloat as int))+max(Plus)) then '1' else 'O nº de rotas para a correia ' + cast(max(L1.Name) as varchar (10)) + ' chegou no limite de ' + cast( max(ValueFloat) as varchar (10)) + ' rota(s).' end 
	from Location as L1
	inner join Location as L2 on L2.ParentId = L1.Id 
	inner join rLocationValue as LVal on LVal.Id = L2.Id
	inner join rRouteGraphSequence as SL on L1.Id = SL.LocationId and L2.TypeId = 61
	inner join rRouteGraphLocation as RGL on SL.RouteGraphId = RGL.Id
	inner join rRouteActive as Acv on Acv.Id = RGL.LocationId
	inner join Location as Route on Route.Id = Acv.Id and Route.Name not like '%pier%' --Elimina rotas ativas com palavra píer no nome devido a duplicidade de rota.
	inner join ( -- Ler qual é o limite para de uma correia de uma rota
	select L1.Id, Plus
	from Location as L1
	inner join Location as L2 on L2.ParentId = L1.Id 
	inner join rLocationValue as LVal on LVal.Id = L2.Id
	inner join rRouteGraphSequence as SL on L1.Id = SL.LocationId and L2.TypeId = 61
	inner join rRouteGraphLocation as RGL on SL.RouteGraphId = RGL.Id
	inner join rRouteQueue as Q on Q.Id = RGL.LocationId
	inner join (select Id, case when Route like '%Limp%' then 1 else 0 end as Plus, Route from RouteGraph) as Limp on Limp.Id = SL.RouteGraphId
	where
	RGL.LocationId = @RouteId
	) as TrLm on TrLm.Id = L1.Id
	left outer join ( -- Verificar se a correia está em uma rota da fila
	select L1.Id, 1 as Qnt
	from Location as L1
	inner join Location as L2 on L2.ParentId = L1.Id 
	inner join rRouteGraphSequence as SL on L1.Id = SL.LocationId and L2.TypeId = 61
	inner join rRouteGraphLocation as RGL on SL.RouteGraphId = RGL.Id
	inner join rRouteQueue as Q on Q.Id = RGL.LocationId
	group by L1.Id
	) as Queue on Queue.Id = L1.Id
	group by L1.Id

	SELECT @consitency = 'Só é permitido 1 rota simultânea por VV. O '+Eqp.Name + ' já atingiu o limite.'  
	FROM [Route].[dbo].[rRouteGraphLocation] as RGL
	inner join rRouteGraphSequence as RGS on RGL.Id = RGS.RouteGraphId and [Order] = 1
	inner join Location as Eqp on Eqp.Id = RGS.LocationId
	inner join rRouteActive as Act on Act.Id = RGL.LocationId
	inner join (
	select Eqp.Id from [Route].[dbo].[rRouteGraphLocation] as RGL
	inner join rRouteGraphSequence as RGS on RGL.Id = RGS.RouteGraphId and [Order] = 1
	inner join Location as Eqp on Eqp.Id = RGS.LocationId
	where
	RGL.LocationId = @RouteId) as O on O.Id = Eqp.Id
	where
	Eqp.TypeId = 39

	return @consitency

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Consistency_I_Route_Tripper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_Consistency_I_Route_Tripper]
(
	-- Add the parameters for the function here
	@RouteId int, @ReplacedId int
)
RETURNS int
AS
BEGIN
	declare @consistency int = 1
	SELECT top 1  @consistency =  CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END
	FROM            (SELECT @RouteId as Id
                          --UNION
                          --SELECT @ReplacedId as Id
						  ) AS ACTIVE INNER JOIN
                         dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
                         dbo.rRouteOxD AS ROXD ON ROXD.Id = IDROUTE.Id INNER JOIN
                         dbo.OxD ON dbo.OxD.Id = ROXD.OxDId INNER JOIN
                         dbo.rRouteSequence AS SEQ ON ACTIVE.Id = SEQ.RouteId INNER JOIN
                         dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
                         dbo.rRouteSequence AS SEQ_EQP_NEXT ON SEQ_EQP_NEXT.RouteId = SEQ.RouteId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
                         dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
                         dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
                         dbo.Tripper AS TP ON TP.Id = MED_CON_L1.Id INNER JOIN
                         dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
                         dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
                         dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
                         dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
                         dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
                         dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id INNER JOIN
                             (SELECT        RouteId, MAX([Order]) AS [Order]
                               FROM            dbo.rRouteSequence
                               GROUP BY RouteId) AS ORDER_D ON ORDER_D.RouteId = ACTIVE.Id

	return @consistency

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Production_RouteAjust_I_Route]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira
-- Create date: 09/05/2024
-- Description:	Altera o destino baseado em alguma informação específica relacionada a ele
-- Caso o destino não tenha informações específicas o retorno será a entrada
-- =============================================
CREATE FUNCTION [dbo].[fn_Production_RouteAjust_I_Route]
(
	-- Add the parameters for the function here
	@RouteId bigint
)
RETURNS bigint
AS
BEGIN

	-- Declare the return variable here
	DECLARE @ResultVar  bigint, @idDestination int, 
	@Eqp_Initial varchar (10), @Eqp_Final varchar (10),
	@RouteStr varchar (max), @LastCoveyorId int

	set @ResultVar = @RouteId

	
	select @LastCoveyorId = LocationId from rRouteSequence as Seq
	inner join (select RouteId, Max([Order]) as [Order] from rRouteSequence 
	inner join Location as LConv on LConv.Id = rRouteSequence.LocationId and LConv.TypeId = 17
				inner join Conveyor on Conveyor.Id = rRouteSequence.LocationId
				where RouteId = @RouteId group by RouteId) as LastC on LastC.RouteId = Seq.RouteId and LastC.[Order] = Seq.[Order]
	where Seq.RouteId = @RouteId 

	select @idDestination = OxD.DestinationId, @RouteStr = Route.Name, @Eqp_Initial = Dest.Name from rRouteOxD
	inner join OxD on OxD.Id = rRouteOxD.OxDId
	inner join Location as Dest on Dest.Id = OxD.DestinationId
	inner join Location as Route on Route.Id = rRouteOxD.Id
	where
	rRouteOxD.Id = @RouteId


	--- Informação específica sobre EP03
	If (@idDestination = 97)
	begin
		select @Eqp_Final =
		case 			
			when Value = '1' then 'EP03 EE01' -- EP03 acomplada com EE01
			when Value = '2' then 'EP03 EE02' -- EP03 acomplada com EE02
			else
				'EP03' -- EP03 desacoplada
		end
		from Location as EP03
		inner join Location as ACOP on ACOP.ParentId = EP03.Id and ACOP.TypeId = 50
		inner join Location as INST on INST.ParentId = ACOP.Id 
		inner join rInstrumentMeasure as InsVal on InsVal.Id = INST.Id
		where
		EP03.Id = @idDestination
		select @ResultVar = Id from Location where Name = REPLACE(@RouteStr,@Eqp_Initial, @Eqp_Final)
	end
	else If (@idDestination = 100 or @LastCoveyorId = 122)
	begin
		select @Eqp_Final =
		case 			
			when Value = '1' then 'TRMH' -- EP10 acomplada com tremonha
			else
				'EP10' -- EP10 desacoplada
		end
		from Location as EP10
		inner join Location as ACOP on ACOP.ParentId = EP10.Id and ACOP.TypeId = 50
		inner join Location as INST on INST.ParentId = ACOP.Id 
		inner join rInstrumentMeasure as InsVal on InsVal.Id = INST.Id
		where
		EP10.Id = 100
		select @ResultVar = Id from Location where Name = REPLACE(@RouteStr,@Eqp_Initial, @Eqp_Final)
	end
	else If (@LastCoveyorId = 12)
	begin
		set @Eqp_Final = 'EPA2A'
		
		select @ResultVar = Id from Location where Name = REPLACE(@RouteStr,@Eqp_Initial, @Eqp_Final)
	end
	else If (@LastCoveyorId = 17)
	begin
		set @Eqp_Final = 'EPA4'
		select @ResultVar = Id from Location where Name = REPLACE(@RouteStr,@Eqp_Initial, @Eqp_Final)
	end

	-- Return the result of the function
	RETURN @ResultVar

END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Route_Consistency_Feeder]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[fn_Route_Consistency_Feeder]
(	
	-- Add the parameters for the function here
	@RotaId int
)
RETURNS 
@tbReturn TABLE 
(
	VwId int
      ,Id int
      ,Route varchar(500)
      ,AssetCurrent varchar(500)
      ,AssetCurrentId int
      ,AssetNext varchar(500)
      ,AssetNextId int
      ,Veredict varchar(500)
      ,BoolVeredict bit
      ,ReferenceId int
      ,Desired varchar(500)
      ,[Current] varchar(500)
      ,TagName varchar(500)
      ,TagId int
      ,Type varchar(500)
      ,FeederId int
)
AS
Begin
	INSERT INTO @tbReturn
           ( [VwId]
      ,[Id]
      ,[Route]
      ,[AssetCurrent]
      ,[AssetCurrentId]
      ,[AssetNext]
      ,[AssetNextId]
      ,[Veredict]
      ,[BoolVeredict]
      ,[ReferenceId]
      ,[Desired]
      ,[Current]
      ,[TagName]
      ,[TagId]
      ,[Type]
      ,[FeederId])

	SELECT        ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, IDROUTE.Id, IDROUTE.Description AS Route, EQP.Alias AS AssetCurrent, EQP.Id AS AssetCurrentId, EQP_NEXT.Alias AS AssetNext, EQP_NEXT.Id AS AssetNextId,
 CASE WHEN REF_POS.Value = MED_CON.Value THEN 'Correct' ELSE 'Wrong' END AS Veredict, CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END AS BoolVeredict, REF_POS.Id AS ReferenceId, 
REF_POS.Value AS Desired, MED_CON.Value AS [Current], '[' + PLC.Alias + ']' + TAG_MED_CON.Name AS TagName, TAG_MED_CON.Id AS TagId, 'Feeder' AS Type, CM.Id AS FeederId
FROM            (SELECT        Id
                          FROM            dbo.rRouteQueue
                          UNION
                          SELECT        Id
                          FROM            dbo.rRouteActive
						  union
						  select @RotaId) AS ACTIVE INNER JOIN
                         dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
                         dbo.rRouteOxD AS ROXD ON ROXD.Id = IDROUTE.Id INNER JOIN
                         dbo.OxD ON dbo.OxD.Id = ROXD.OxDId INNER JOIN
                         dbo.rRouteSequence AS SEQ ON ACTIVE.Id = SEQ.RouteId INNER JOIN
                         dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
                         dbo.rRouteSequence AS SEQ_EQP_NEXT ON SEQ_EQP_NEXT.RouteId = SEQ.RouteId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
                         dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
                         dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
                         dbo.Feeder AS CM ON CM.Id = MED_CON_L1.Id INNER JOIN
                         dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
                         Consistency AS CON ON CON.Id = MED_CON_L2.Id INNER JOIN
                         dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
                         dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
                         dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
                         dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
                         dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id

	Return

end
GO
/****** Object:  UserDefinedFunction [dbo].[fn_RouteReplacer_I_Route_O_Route]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira
-- Create date: 17/11/2021
-- Description:	Filtro para lista de rotas para subsituição de rotas
-- =============================================
CREATE FUNCTION [dbo].[fn_RouteReplacer_I_Route_O_Route]
(	
	-- Add the parameters for the function here
	@RouteId int
)
RETURNS 
@tbReturn TABLE 
(
	Id int
      ,GPVCode int
      ,Route varchar(500)
      ,Reduced varchar(500)
      ,Rules varchar(500)
      ,Consistency int
     
)
AS
Begin

	INSERT INTO @tbReturn
           ( [Id]
      ,GpvCode
      ,[Route]
      ,[Reduced]
      ,[Rules]
      ,[Consistency])
	SELECT Route.Id,
	0 as GpvCode, 
	Route.Name as 'Route', 
	Route.Alias as Reduced, 
	[dbo].[fn_Consistency_I_Route_Rule] (@RouteId) as Rules,
	case when [dbo].fn_Consistency_I_Route_Feeder(Route.Id,@RouteId) = 1 and
	[dbo].fn_Consistency_I_Route_Reversal(Route.Id,@RouteId) = 1 and
	[dbo].fn_Consistency_I_Route_Tripper(Route.Id,@RouteId) = 1 and
	[dbo].fn_Consistency_I_Route_Damper(Route.Id,@RouteId) = 1 then 1 else 0 end as Consistency
	FROM 
	(
	select Location.Id, Location.Name, Location.Alias, Location.Description from Location
	inner join
	( -- Esta subquery seleciona somente as rotas elegíveis a substituir a rota substituida
	  -- As rotas elegíveis são as que possuem o mesmo caminho de destino a partir da 3ª correia transportadora
		SELECT 
		SUBSTRING(Route, CHARINDEX(Name,Route)+LEN(Name)+1,LEN(Route)-CHARINDEX(Name,Route)+LEN(Name)) as Path, 
		SUBSTRING(Route, 0,CHARINDEX(Name,Route)+LEN(Name)+1) as Origin, Route
		FROM [Route].[dbo].[rRouteGraphLocation] as R
		inner join rRouteGraphSequence as Seq on Seq.RouteGraphId = R.Id
		inner join RouteGraph as G on G.Id = R.Id
		inner join Location as L on L.Id = Seq.LocationId and Seq.[Order] = 2
		where
		R.LocationId = @RouteId) as Path on 
		Location.Name like '%'+Path.Path and 
		Location.Name not like Path.Origin + '%'
		and TypeId <> 12
	) as Route
	Return

end
GO
/****** Object:  Table [dbo].[rRouteQueue]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteQueue](
	[Id] [bigint] NOT NULL,
	[dh] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [bigint] NOT NULL,
	[ParentId] [bigint] NULL,
	[TypeId] [bigint] NOT NULL,
	[Name] [varchar](max) NULL,
	[Alias] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Consistency_Rule]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency_Rule]
AS
SELECT        ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, 
IDROUTE.Id, IDROUTE.Name AS Route, 
[dbo].[fn_Consistency_I_Route_Rule](IDROUTE.Id) AS Veredict, 
case when [dbo].[fn_Consistency_I_Route_Rule](IDROUTE.Id) = '1' then 1 else 0 end AS BoolVeredict
FROM            (SELECT        Id
                          FROM            dbo.rRouteQueue) AS ACTIVE INNER JOIN
                         dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id
GO
/****** Object:  Table [dbo].[rInstrumentMeasure]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rInstrumentMeasure](
	[Id] [bigint] NOT NULL,
	[Value] [nvarchar](max) NULL,
	[dh] [datetime] NOT NULL,
	[LastDh] [datetime] NOT NULL,
	[isUpdating] [bit] NOT NULL,
	[LastValue] [nvarchar](max) NULL,
	[TagId] [bigint] NOT NULL,
	[ErrorCode] [nvarchar](max) NULL,
	[ErrorString] [nvarchar](max) NULL,
	[Write] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.rInstrumentMeasure] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteGraphLocation]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteGraphLocation](
	[Id] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteGraphLocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteGraphSequence]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteGraphSequence](
	[Id] [bigint] NOT NULL,
	[Order] [int] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[RouteGraphId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteGraphSequence] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteActive]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteActive](
	[Id] [bigint] NOT NULL,
	[dh] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteActive] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Eqp_RouteActive]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Eqp_RouteActive]
AS
SELECT        Seq.LocationId AS Id, COUNT(*) AS Qnt
FROM            dbo.rRouteActive AS active INNER JOIN
                         dbo.rRouteGraphLocation AS rgl ON rgl.LocationId = active.Id INNER JOIN
                         dbo.rRouteGraphSequence AS Seq ON Seq.RouteGraphId = rgl.Id
GROUP BY Seq.LocationId
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PlcId] [bigint] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Tag] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Command]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Command](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Command] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reference]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reference](
	[Id] [bigint] NOT NULL,
	[LocReferenceId] [bigint] NULL,
	[Value] [float] NOT NULL,
 CONSTRAINT [PK_dbo.Reference] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reversal]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reversal](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Reversal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Consistency_Reversal]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency_Reversal]
AS
SELECT ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, IDROUTE.Id, IDROUTE.Name AS Route, EQP.Alias AS AssetCurrent, EQP.Id AS AssetCurrentId, EQP_NEXT.Alias AS AssetNext, EQP_NEXT.Id AS AssetNextId, CASE WHEN EqpAct.Qnt IS NULL 
THEN 'Correct' WHEN EqpAct.Qnt IS NOT NULL AND REF_POS.Value = MED_CON.Value THEN 'Correct' ELSE 'Wrong' END AS Veredict, CASE WHEN EqpAct.Qnt IS NULL THEN 1 WHEN EqpAct.Qnt IS NOT NULL AND REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END AS BoolVeredict, 
REF_POS.Id AS ReferenceId, REF_POS.Value AS Desired, MED_CON.Value AS [Current], '[' + PLC.Alias + ']' + TAG_MED_CON.Name AS TagName, TAG_MED_CON.Id AS TagId, 'Reversal' AS Type, Active.Active
FROM   (SELECT Id, 0 as Active
             FROM    dbo.rRouteQueue
             UNION
             SELECT Id, 1 as Active
             FROM   dbo.rRouteActive) AS ACTIVE INNER JOIN
             dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphLocation AS RGL_1 ON RGL_1.LocationId = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
             dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
             dbo.rRouteGraphSequence AS SEQ_EQP_NEXT ON SEQ.RouteGraphId = SEQ_EQP_NEXT.RouteGraphId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
             dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
             dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
             dbo.Reversal AS RV ON RV.Id = MED_CON_L1.Id INNER JOIN
             dbo.Location AS RV_L2 ON RV_L2.ParentId = MED_CON_L1.Id INNER JOIN
             dbo.Location AS RV_L3 ON RV_L3.ParentId = RV_L2.Id INNER JOIN
             dbo.Reference AS REF_POS ON REF_POS.Id = RV_L3.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id INNER JOIN
             dbo.Location AS RV_L4 ON RV_L4.ParentId = RV_L3.Id INNER JOIN
             dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = RV_L4.Id INNER JOIN
             dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id LEFT OUTER JOIN
             vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = EQP.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Reversal_Command]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Reversal_Command]
AS
SELECT DISTINCT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Reversal command' AS Type, REF_L1.Name
FROM            dbo.vw_Route_Consistency_Reversal AS vw INNER JOIN
                         dbo.Location AS REF_L1 ON REF_L1.ParentId = vw.ReferenceId INNER JOIN
                         dbo.Location AS REF_L2 ON REF_L2.ParentId = REF_L1.Id INNER JOIN
                         dbo.Command AS CMD ON CMD.Id = REF_L1.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = REF_L2.Id INNER JOIN
                         dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id LEFT OUTER JOIN
                         dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId
WHERE        (EqpAct.Id IS NULL)
GO
/****** Object:  Table [dbo].[Tripper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tripper](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Tripper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Consistency_Tripper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency_Tripper]
AS
SELECT ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, IDROUTE.Id, IDROUTE.Name AS Route, EQP.Alias AS AssetCurrent, EQP.Id AS AssetCurrentId, EQP_NEXT.Alias AS AssetNext, EQP_NEXT.Id AS AssetNextId, CASE WHEN REF_POS.Value = MED_CON.Value OR
RC_IG.Id IS NOT NULL THEN 'Correct' ELSE 'Wrong' END AS Veredict, CASE WHEN REF_POS.Value = MED_CON.Value OR
RC_IG.Id IS NOT NULL THEN 1 ELSE 0 END AS BoolVeredict, CASE WHEN
    ((SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
     FROM    dbo.rRouteGraphLocation AS RGL_1 INNER JOIN
                  dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
                  rRouteQueue ON rRouteQueue.Id = RGL_1.LocationId INNER JOIN
                  Location AS MED_CON_L1 ON MED_CON_L1.ParentId = SEQ.LocationId INNER JOIN
                  Tripper AS TP ON TP.Id = MED_CON_L1.Id
     WHERE [Order] = 2) +
    (SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
    FROM    dbo.rRouteGraphLocation AS RGL_1 INNER JOIN
                 dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
                 rRouteQueue ON rRouteQueue.Id = RGL_1.LocationId INNER JOIN
                 Location AS MED_CON_L1 ON MED_CON_L1.ParentId = SEQ.LocationId INNER JOIN
                 Tripper AS TP ON TP.Id = MED_CON_L1.Id
    WHERE [Order] = ORDER_D.[Order] - 1)) > 1 THEN 4 WHEN SEQ.[Order] = ORDER_D.[Order] - 1 THEN 2 WHEN SEQ.[Order] = 2 THEN 3 ELSE 1 END AS Mode, CASE WHEN
    ((SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
     FROM    dbo.rRouteGraphLocation AS RGL_1 INNER JOIN
                  dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
                  rRouteQueue ON rRouteQueue.Id = RGL_1.LocationId INNER JOIN
                  Location AS MED_CON_L1 ON MED_CON_L1.ParentId = SEQ.LocationId INNER JOIN
                  Tripper AS TP ON TP.Id = MED_CON_L1.Id
     WHERE [Order] = 2) +
    (SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
    FROM    dbo.rRouteGraphLocation AS RGL_1 INNER JOIN
                 dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
                 rRouteQueue ON rRouteQueue.Id = RGL_1.LocationId INNER JOIN
                 Location AS MED_CON_L1 ON MED_CON_L1.ParentId = SEQ.LocationId INNER JOIN
                 Tripper AS TP ON TP.Id = MED_CON_L1.Id
    WHERE [Order] = ORDER_D.[Order] - 1)) > 1 THEN 'Stacking and Reclaiming' WHEN SEQ.[Order] = ORDER_D.[Order] - 1 THEN 'Stacking' WHEN SEQ.[Order] = 2 THEN 'Reclaiming' ELSE 'Direct Path' END AS DescriptionMode, REF_POS.Id AS ReferenceId, REF_POS.Value AS Desired, 
MED_CON.Value AS [Current], '[' + PLC.Alias + ']' + TAG_MED_CON.Name AS TagName, TAG_MED_CON.Id AS TagId, 'Tripper' AS Type, Active.Active
FROM   (SELECT Id, 0 AS Active
             FROM    dbo.rRouteQueue
             UNION
             SELECT Id, 1 AS Active
             FROM   dbo.rRouteActive) AS ACTIVE INNER JOIN
             dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphLocation AS RGL_1 ON RGL_1.LocationId = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
             dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
             dbo.rRouteGraphSequence AS SEQ_EQP_NEXT ON SEQ.RouteGraphId = SEQ_EQP_NEXT.RouteGraphId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
             dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
             dbo.rRouteGraphSequence AS SEQ_EQP_BEF ON SEQ.RouteGraphId = SEQ_EQP_BEF.RouteGraphId AND SEQ_EQP_BEF.[Order] = SEQ.[Order] - 1 INNER JOIN
             dbo.Location AS EQP_BEF ON EQP_BEF.Id = SEQ_EQP_BEF.LocationId INNER JOIN
             dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
             dbo.Tripper AS TP ON TP.Id = MED_CON_L1.Id INNER JOIN
             dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
             dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
             dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
             dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
             dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND ((REF_POS.LocReferenceId = EQP_NEXT.Id AND REF_L1.TypeId = 58 AND SEQ.[Order] > 2) OR
             (REF_POS.LocReferenceId = EQP_NEXT.Id AND REF_L1.TypeId = 59) OR
             (REF_POS.LocReferenceId = EQP_BEF.Id) AND REF_L1.TypeId = 60 AND SEQ.[Order] = 2) INNER JOIN
                 (SELECT RGL_1.LocationId AS RouteId, MAX([Order]) AS [Order]
                 FROM    dbo.rRouteGraphLocation AS RGL_1 INNER JOIN
                              dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id
                 GROUP BY RGL_1.LocationId) AS ORDER_D ON ORDER_D.RouteId = ACTIVE.Id LEFT OUTER JOIN
             dbo.Location AS RC_IG ON RC_IG.ParentId = EQP_BEF.Id AND RC_IG.TypeId = 64
GO
/****** Object:  View [dbo].[vw_Route_Tag_Tripper_Command]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Tripper_Command]
AS
SELECT DISTINCT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Tripper command' AS Type, REF_L1.Name
FROM            dbo.vw_Route_Consistency_Tripper AS vw INNER JOIN
                         dbo.Location AS REF_L1 ON REF_L1.ParentId = vw.ReferenceId INNER JOIN
                         dbo.Location AS REF_L2 ON REF_L2.ParentId = REF_L1.Id INNER JOIN
                         dbo.Command AS CMD ON CMD.Id = REF_L1.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = REF_L2.Id INNER JOIN
                         dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id INNER JOIN
                         dbo.Location AS REF_OK ON REF_OK.Id = vw.ReferenceId INNER JOIN
                         dbo.Location AS CON ON CON.Id = REF_OK.ParentId INNER JOIN
                         dbo.Location AS REF_ALL ON REF_ALL.ParentId = CON.Id AND REF_ALL.Id <> REF_OK.Id LEFT OUTER JOIN
                         dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId
WHERE        (EqpAct.Id IS NULL)
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Tag_Tripper_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Tripper_Permission]
AS
SELECT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Tripper permission' AS Type, REF_L1.Name
FROM   dbo.vw_Route_Consistency_Tripper AS vw INNER JOIN
             dbo.Location AS REF_L1 ON REF_L1.ParentId = vw.ReferenceId INNER JOIN
             dbo.Location AS REF_L2 ON REF_L2.ParentId = REF_L1.Id INNER JOIN
             dbo.Permission AS CMD ON CMD.Id = REF_L1.Id INNER JOIN
             dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = REF_L2.Id INNER JOIN
             dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct2 ON EqpAct2.Id = vw.AssetNextId AND vw.Active = 1
GO
/****** Object:  View [dbo].[vw_Route_Tag_Eqp_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Eqp_Permission]
AS
SELECT        RGL.LocationId AS Id, dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, MEAS.Value AS TagValue, 'Eqp permission' AS Type, EQP_L1.Name
FROM            dbo.rRouteActive AS ACTIVE INNER JOIN
                         dbo.rRouteGraphLocation AS RGL ON RGL.LocationId = ACTIVE.Id INNER JOIN
                         dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL.Id INNER JOIN
                         dbo.Location AS EQP_L1 ON EQP_L1.ParentId = SEQ.LocationId INNER JOIN
                         dbo.Permission AS EQP_PERM ON EQP_PERM.Id = EQP_L1.Id INNER JOIN
                         dbo.Location AS EQP_L2 ON EQP_L2.ParentId = EQP_L1.Id AND EQP_L2.TypeId = 4 INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = EQP_L2.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
GO
/****** Object:  Table [dbo].[rRouteOxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteOxD](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OxDId] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteOxD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Tag_OxD_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_OxD_Permission]
AS
SELECT        RGL.LocationId AS Id, dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, MEAS.Value AS TagValue, 'OxD permission' AS Type, EQP_L1.Name
FROM            dbo.rRouteActive AS ACTIVE INNER JOIN
                         dbo.rRouteGraphLocation AS RGL ON RGL.LocationId = ACTIVE.Id INNER JOIN
                         dbo.rRouteOxD AS RX ON RX.LocationId = RGL.LocationId INNER JOIN
                         dbo.Location AS EQP_L1 ON EQP_L1.ParentId = RX.OxDId INNER JOIN
                         dbo.Permission AS EQP_PERM ON EQP_PERM.Id = EQP_L1.Id INNER JOIN
                         dbo.Location AS EQP_L2 ON EQP_L2.ParentId = EQP_L1.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = EQP_L2.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
GO
/****** Object:  Table [dbo].[rLocationValue]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLocationValue](
	[Id] [bigint] NOT NULL,
	[ValueFloat] [real] NULL,
	[ValueString] [nvarchar](max) NULL,
	[ValueBool] [bit] NULL,
	[ValueDateTime] [datetime] NULL,
	[ValueInt] [int] NULL,
 CONSTRAINT [PK_dbo.rLocationValue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteSequence]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteSequence](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Order] [int] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[RouteId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteSequence] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Origin_Time_To_Scale]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Origin_Time_To_Scale]
AS
SELECT        active.Id, Bal.Name, Time.ValueString AS Time
FROM            dbo.rRouteActive AS active INNER JOIN
                         dbo.rRouteSequence AS seq ON seq.RouteId = active.Id INNER JOIN
                         dbo.Location AS tr ON tr.Id = seq.LocationId AND (tr.TypeId = 17 OR
                         tr.Id = 100) INNER JOIN
                         dbo.Location AS Bal ON Bal.ParentId = tr.Id AND Bal.TypeId = 26 INNER JOIN
                         dbo.Location AS BalTime ON BalTime.ParentId = Bal.Id INNER JOIN
                         dbo.rLocationValue AS Time ON Time.Id = BalTime.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Reversal_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Reversal_Permission]
AS
SELECT DISTINCT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Reversal permission' AS Type, REF_L1.Name
FROM   dbo.vw_Route_Consistency_Reversal AS vw INNER JOIN
             dbo.Location AS REF_L1 ON REF_L1.ParentId = vw.ReferenceId INNER JOIN
             dbo.Location AS REF_L2 ON REF_L2.ParentId = REF_L1.Id INNER JOIN
             dbo.Permission AS CMD ON CMD.Id = REF_L1.Id INNER JOIN
             dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = REF_L2.Id INNER JOIN
             dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id INNER JOIN
             dbo.rRouteActive AS ACTIVE ON ACTIVE.Id = vw.Id INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct2 ON EqpAct2.Id = vw.AssetNextId AND vw.Active = 1
GO
/****** Object:  Table [dbo].[RouteGraph]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RouteGraph](
	[Id] [bigint] NOT NULL,
	[Route] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.RouteGraph] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_VV_Limp_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_VV_Limp_Permission]
AS
SELECT        RGL.LocationId AS Id, dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, MEAS.Value AS TagValue, 'VV Limp Permission' AS Type, EQP_L1.Name
FROM            dbo.rRouteActive AS ACTIVE INNER JOIN
                         dbo.rRouteGraphLocation AS RGL ON RGL.LocationId = ACTIVE.Id INNER JOIN
                         dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL.Id INNER JOIN
                         dbo.RouteGraph AS ROUTE ON ROUTE.Id = RGL.Id AND ROUTE.Route LIKE '%Manob/Limp%' INNER JOIN
                         dbo.Location AS EQP_L1 ON EQP_L1.ParentId = SEQ.LocationId AND EQP_L1.TypeId = 66 INNER JOIN
                         dbo.Location AS EQP_L2 ON EQP_L2.ParentId = EQP_L1.Id AND EQP_L2.TypeId = 9 INNER JOIN
                         dbo.Permission AS EQP_PERM ON EQP_PERM.Id = EQP_L2.Id INNER JOIN
                         dbo.Location AS EQP_L3 ON EQP_L3.ParentId = EQP_L2.Id AND EQP_L3.TypeId = 4 INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = EQP_L3.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
GO
/****** Object:  Table [dbo].[Feeder]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feeder](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Feeder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consistency]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consistency](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Consistency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Consistency_Feeder]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency_Feeder]
AS
SELECT ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, IDROUTE.Id, IDROUTE.Name AS Route, EQP.Alias AS AssetCurrent, EQP.Id AS AssetCurrentId, EQP_NEXT.Alias AS AssetNext, EQP_NEXT.Id AS AssetNextId, 
CASE WHEN REF_POS.Value = MED_CON.Value THEN 'Correct' ELSE 'Wrong' END AS Veredict, CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 0 END AS BoolVeredict, REF_POS.Id AS ReferenceId, REF_POS.Value AS Desired, MED_CON.Value AS [Current], 
'[' + PLC.Alias + ']' + TAG_MED_CON.Name AS TagName, TAG_MED_CON.Id AS TagId, 'Feeder' AS Type, CM.Id AS FeederId, Active.Active
FROM   (SELECT Id, 0 as Active
             FROM    dbo.rRouteQueue
             UNION
             SELECT Id, 1 as Active
             FROM   dbo.rRouteActive) AS ACTIVE INNER JOIN
             dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphLocation AS RGL_1 ON RGL_1.LocationId = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
             dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
             dbo.rRouteGraphSequence AS SEQ_EQP_NEXT ON SEQ.RouteGraphId = SEQ_EQP_NEXT.RouteGraphId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 1 INNER JOIN
             dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
             dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
             dbo.Feeder AS CM ON CM.Id = MED_CON_L1.Id INNER JOIN
             dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
             Consistency AS CON ON CON.Id = MED_CON_L2.Id INNER JOIN
             dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
             dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
             dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
             dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Feeder_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Feeder_Permission]
AS
SELECT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, REFVAL.Value AS Reference, 'Feeder permission' AS Type, CM_L1.Name
FROM   dbo.vw_Route_Consistency_Feeder AS vw INNER JOIN
             dbo.Location AS CM ON CM.Id = vw.FeederId INNER JOIN
             dbo.Location AS CM_L1 ON CM_L1.ParentId = CM.Id INNER JOIN
             dbo.Permission AS CMD ON CMD.Id = CM_L1.Id INNER JOIN
             dbo.Location AS CMD_REF ON CMD_REF.ParentId = CMD.Id INNER JOIN
             dbo.Reference AS REFVAL ON REFVAL.Id = CMD_REF.Id AND REFVAL.LocReferenceId = vw.AssetNextId INNER JOIN
             dbo.Location AS CMD_INS ON CMD_INS.ParentId = REFVAL.Id INNER JOIN
             dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = CMD_INS.Id INNER JOIN
             dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct2 ON EqpAct2.Id = vw.AssetNextId AND vw.Active = 1
GO
/****** Object:  Table [dbo].[Damper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Damper](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Damper] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Consistency_Damper]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency_Damper]
AS
SELECT ROW_NUMBER() OVER (ORDER BY IDROUTE.Id DESC) AS VwId, IDROUTE.Id, IDROUTE.Name AS Route, EQP.Alias AS AssetCurrent, EQP.Id AS AssetCurrentId, EQP_NEXT.Alias AS AssetNext, EQP_NEXT.Id AS AssetNextId, 
CASE WHEN REF_POS.Value = MED_CON.Value THEN 'Correct' ELSE 'Correct' END AS Veredict, CASE WHEN REF_POS.Value = MED_CON.Value THEN 1 ELSE 1 END AS BoolVeredict, REF_POS.Id AS ReferenceId, REF_POS.Value AS Desired, MED_CON.Value AS [Current], 
'[' + PLC.Alias + ']' + TAG_MED_CON.Name AS TagName, TAG_MED_CON.Id AS TagId, 'Damper' AS Type, CM.Id AS DamperId, Active.Active
FROM   (SELECT Id, 0 as Active
             FROM    dbo.rRouteQueue
             UNION
             SELECT Id, 1 as Active
             FROM   dbo.rRouteActive) AS ACTIVE INNER JOIN
             dbo.Location AS IDROUTE ON IDROUTE.Id = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphLocation AS RGL_1 ON RGL_1.LocationId = ACTIVE.Id INNER JOIN
             dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL_1.Id INNER JOIN
             dbo.Location AS EQP ON EQP.Id = SEQ.LocationId INNER JOIN
             dbo.rRouteGraphSequence AS SEQ_EQP_NEXT ON SEQ.RouteGraphId = SEQ_EQP_NEXT.RouteGraphId AND SEQ_EQP_NEXT.[Order] = SEQ.[Order] + 2 INNER JOIN
             dbo.Location AS EQP_NEXT ON EQP_NEXT.Id = SEQ_EQP_NEXT.LocationId INNER JOIN
             dbo.Location AS MED_CON_L1 ON MED_CON_L1.ParentId = EQP.Id INNER JOIN
             dbo.Damper AS CM ON CM.Id = MED_CON_L1.Id INNER JOIN
             dbo.Location AS MED_CON_L2 ON MED_CON_L2.ParentId = MED_CON_L1.Id INNER JOIN
             Consistency AS CON ON CON.Id = MED_CON_L2.Id INNER JOIN
             dbo.Location AS MED_CON_L3 ON MED_CON_L3.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.rInstrumentMeasure AS MED_CON ON MED_CON.Id = MED_CON_L3.Id INNER JOIN
             dbo.Tag AS TAG_MED_CON ON TAG_MED_CON.Id = MED_CON.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_MED_CON.PlcId = PLC.Id INNER JOIN
             dbo.Location AS LOC_MED_CON ON LOC_MED_CON.Id = MED_CON.Id INNER JOIN
             dbo.Location AS REF_L1 ON REF_L1.ParentId = MED_CON_L2.Id INNER JOIN
             dbo.Reference AS REF_POS ON REF_POS.Id = REF_L1.Id AND REF_POS.LocReferenceId = EQP_NEXT.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Damper_Command]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Damper_Command]
AS
SELECT        TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Damper Command' AS Type, DP_CMD.Name
FROM            dbo.vw_Route_Consistency_Damper AS vw INNER JOIN
                         dbo.Location AS DP_CMD ON DP_CMD.ParentId = vw.DamperId AND DP_CMD.TypeId = 10 INNER JOIN
                         dbo.Location AS DP_REF ON DP_REF.ParentId = DP_CMD.Id AND DP_REF.TypeId = 8 INNER JOIN
                         dbo.Reference AS REF ON REF.Id = DP_REF.Id AND REF.LocReferenceId = vw.AssetNextId INNER JOIN
                         dbo.Location AS DP_INS ON DP_INS.ParentId = DP_REF.Id AND DP_INS.TypeId = 4 INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = DP_INS.Id INNER JOIN
                         dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id LEFT OUTER JOIN
                         dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId
WHERE        (EqpAct.Id IS NULL)
GO
/****** Object:  View [dbo].[vw_Route_Tag_Damper_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Damper_Permission]
AS
SELECT TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, 'Damper Permission' AS Type, DP_CMD.Name
FROM   dbo.vw_Route_Consistency_Damper AS vw INNER JOIN
             dbo.Location AS DP_CMD ON DP_CMD.ParentId = vw.DamperId AND DP_CMD.TypeId = 9 INNER JOIN
             dbo.Location AS DP_REF ON DP_REF.ParentId = DP_CMD.Id AND DP_REF.TypeId = 8 INNER JOIN
             dbo.Reference AS REF ON REF.Id = DP_REF.Id AND REF.LocReferenceId = vw.AssetNextId INNER JOIN
             dbo.Location AS DP_INS ON DP_INS.ParentId = DP_REF.Id AND DP_INS.TypeId = 4 INNER JOIN
             dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = DP_INS.Id INNER JOIN
             dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
             dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId INNER JOIN
             dbo.vw_Eqp_RouteActive AS EqpAct2 ON EqpAct2.Id = vw.AssetNextId AND vw.Active = 1
GO
/****** Object:  View [dbo].[vw_Route_Tag_Eqp_O_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Eqp_O_Permission]
AS
SELECT        RGL.LocationId AS Id, dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, MEAS.Value AS TagValue, 'Eqp origin permission' AS Type, EQP_L1.Name
FROM            dbo.rRouteActive AS ACTIVE INNER JOIN
                         dbo.rRouteGraphLocation AS RGL ON RGL.LocationId = ACTIVE.Id INNER JOIN
                         dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL.Id INNER JOIN
                         dbo.Location AS EQP_L1 ON EQP_L1.ParentId = SEQ.LocationId AND EQP_L1.TypeId = 62 INNER JOIN
                         dbo.Location AS EQP_L2 ON EQP_L2.ParentId = EQP_L1.Id INNER JOIN
                         dbo.Permission AS EQP_PERM ON EQP_PERM.Id = EQP_L2.Id INNER JOIN
                         dbo.Location AS EQP_L3 ON EQP_L3.ParentId = EQP_L2.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = EQP_L3.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Eqp_D_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Eqp_D_Permission]
AS
SELECT        RGL.LocationId AS Id, dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, MEAS.Value AS TagValue, 'Eqp permission' AS Type, EQP_L1.Name
FROM            dbo.rRouteActive AS ACTIVE INNER JOIN
                         dbo.rRouteGraphLocation AS RGL ON RGL.LocationId = ACTIVE.Id INNER JOIN
                         dbo.rRouteGraphSequence AS SEQ ON SEQ.RouteGraphId = RGL.Id INNER JOIN
                         dbo.Location AS EQP_L1 ON EQP_L1.ParentId = SEQ.LocationId AND EQP_L1.TypeId = 63 INNER JOIN
                         dbo.Location AS EQP_L2 ON EQP_L2.ParentId = EQP_L1.Id INNER JOIN
                         dbo.Permission AS EQP_PERM ON EQP_PERM.Id = EQP_L2.Id INNER JOIN
                         dbo.Location AS EQP_L3 ON EQP_L3.ParentId = EQP_L2.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = EQP_L3.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
WHERE        (dbo.Tag.Id = - 6)
GO
/****** Object:  View [dbo].[vw_Tag_Bool_Activate]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Tag_Bool_Activate]
AS
/****** Script for SelectTopNRows command from SSMS  ******/ SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Damper_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Eqp_D_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Eqp_O_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Eqp_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Feeder_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_OxD_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Reversal_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Tripper_Permission]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_Tag_Damper_Command]
UNION
SELECT DISTINCT [TagId]
FROM            [Route].[dbo].[vw_Route_VV_Limp_Permission]
GO
/****** Object:  View [dbo].[vw_Spv_Rotascco_RC04]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Spv_Rotascco_RC04]
AS
SELECT dbo.rRouteActive.Id, dbo.rRouteActive.dh, dbo.Location.Id AS Expr1, dbo.Location.ParentId, dbo.Location.TypeId, dbo.Location.Name, dbo.Location.Alias, dbo.Location.Description
FROM   dbo.rRouteActive INNER JOIN
             dbo.Location ON dbo.Location.Id = dbo.rRouteActive.Id
WHERE (dbo.Location.Name LIKE '%rc04%') AND (dbo.Location.Name NOT LIKE '%cn%')
GO
/****** Object:  Table [dbo].[Route]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Route](
	[Id] [bigint] NOT NULL,
	[GpvCode] [bigint] NOT NULL,
	[Inactive] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Route] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Scale]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Scale]
AS
SELECT        dbo.Route.Id AS RouteId, dbo.Route.GpvCode, LocR.Name AS Route, dbo.Tag.Name AS TagName, dbo.Tag.Id AS TagId, L1.Name AS EqpBal, L1.Id AS BalId, seq.[Order], L2.Id AS VazTotId, L2.Name AS VazTot, 
                         L3.Name AS Instrument, L3.Id AS InstrumentId, CAST(InsVal.Value AS float) AS Value, L2.TypeId, seq.LocationId
FROM            dbo.rRouteActive INNER JOIN
                         dbo.Route ON dbo.Route.Id = dbo.rRouteActive.Id INNER JOIN
                         dbo.Location AS LocR ON LocR.Id = dbo.Route.Id INNER JOIN
                         dbo.rRouteSequence AS seq ON seq.RouteId = dbo.rRouteActive.Id INNER JOIN
                         dbo.Location AS L1 ON L1.ParentId = seq.LocationId AND L1.TypeId = 26 INNER JOIN
                         dbo.Location AS L2 ON L2.ParentId = L1.Id INNER JOIN
                         dbo.Location AS L3 ON L3.ParentId = L2.Id INNER JOIN
                         dbo.rInstrumentMeasure AS InsVal ON InsVal.Id = L3.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = InsVal.TagId
GO
/****** Object:  View [dbo].[vw_Spv_Rotascco_RC05]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[vw_Spv_Rotascco_RC05]
AS
SELECT rra.Id, rra.dh, loc.Id AS Expr1, loc.ParentId, loc.TypeId, loc.Name, loc.Alias, loc.Description
FROM   dbo.rRouteActive AS rra INNER JOIN
             dbo.Location AS loc ON loc.Id = rra.Id
WHERE (loc.Name LIKE '%rc05%') AND (loc.Name NOT LIKE '%cn%')
GO
/****** Object:  Table [dbo].[OxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OxD](
	[Id] [bigint] NOT NULL,
	[OriginId] [bigint] NULL,
	[DestinationId] [bigint] NULL,
 CONSTRAINT [PK_dbo.OxD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_Scale_Destination]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Scale_Destination]
AS
with cte 
as
(
SELECT        ROW_NUMBER() OVER (partition BY Bal.RouteId
ORDER BY bal.Value DESC) AS rn, Bal.BalId, Bal.InstrumentId, TagId, Bal.Value, Bal.VazTot, Bal.RouteId, dbo.OxD.DestinationId
FROM            dbo.vw_Route_Scale AS Bal INNER JOIN
                         dbo.rRouteSequence AS Seq ON Seq.RouteId = Bal.RouteId INNER JOIN
                         dbo.rRouteOxD ON dbo.rRouteOxD.Id = Seq.RouteId INNER JOIN
                         dbo.OxD ON dbo.OxD.Id = dbo.rRouteOxD.OxDId INNER JOIN
                             (SELECT   T1.RouteId, MAX(T1.[Order]) AS [Order]
                               FROM            dbo.rRouteSequence AS T1 INNER JOIN
                                                         dbo.vw_Route_Scale AS T2 ON T1.RouteId = T2.RouteId INNER JOIN
                                                         dbo.Location AS Bal ON Bal.Id = T2.BalId INNER JOIN
														(
														select RouteId, count (*) as ct from
														(
														SELECT distinct [BalId], RouteId  FROM [Route].[dbo].[vw_Route_Scale] as vw
														INNER JOIN Location ON Location.id = vw.LocationId
														WHERE        vw.TypeId = 42 and Location.TypeId <> 39 and Location.TypeId <> 33
														) as T1
														group by RouteId ) as EP ON EP.RouteId = T1.RouteId INNER JOIN
                                                         dbo.Location AS EqpBal ON EqpBal.Id = Bal.ParentId AND T1.LocationId = EqpBal.Id AND (EqpBal.TypeId = 17 OR
                                                         EqpBal.TypeId = 36 OR
                                                         (EP.ct = 1 AND EqpBal.TypeId = 31))
                               GROUP BY T1.RouteId) AS OrderBal ON OrderBal.RouteId = Seq.RouteId AND OrderBal.[Order] = Seq.[Order] AND bal.[Order] = OrderBal.[Order]
WHERE        (Bal.TypeId = 42))
    SELECT        DestinationId, BalId, TagId, InstrumentId, Value, VazTot, RouteId
     FROM            cte
     WHERE        rn = 1
GO
/****** Object:  View [dbo].[vw_Spv_Rotascco_Pier2]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Spv_Rotascco_Pier2]
AS
SELECT dbo.rRouteActive.Id, dbo.rRouteActive.dh, dbo.Location.Id AS Expr1, dbo.Location.ParentId, dbo.Location.TypeId, dbo.Location.Name, dbo.Location.Alias, dbo.Location.Description
FROM   dbo.rRouteActive INNER JOIN
             dbo.Location ON dbo.Location.Id = dbo.rRouteActive.Id
WHERE (dbo.Location.Name LIKE '%pier2%')
GO
/****** Object:  View [dbo].[vw_Route_Scale_Origin]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* Subquery que realiza a leitura das balanças do porto
 Filtra somente as totalização (TypeId = 42) de origens (OxD.OriginId)*/
CREATE VIEW [dbo].[vw_Route_Scale_Origin]
AS
SELECT        Bal.BalId, Bal.InstrumentId, CAST(Bal.Value AS float) AS Value, Bal.VazTot, Bal.RouteId, Bal.TagId
FROM            dbo.vw_Route_Scale AS Bal INNER JOIN
                         dbo.rRouteSequence AS Seq ON Seq.RouteId = Bal.RouteId INNER JOIN
                             (SELECT        T1.RouteId, MIN(T1.[Order]) AS [Order]
                               FROM            dbo.rRouteSequence AS T1 INNER JOIN
                                                         dbo.vw_Route_Scale AS T2 ON T1.RouteId = T2.RouteId INNER JOIN
                                                         dbo.Location AS Bal ON Bal.Id = T2.BalId INNER JOIN
                                                         dbo.Location AS EqpBal ON EqpBal.Id = Bal.ParentId AND T1.LocationId = EqpBal.Id
                               GROUP BY T1.RouteId) AS OrderBal ON OrderBal.RouteId = Seq.RouteId AND OrderBal.[Order] = Seq.[Order] INNER JOIN
                         dbo.Location AS LO ON LO.Id = Seq.LocationId INNER JOIN
                         dbo.Location AS LBAL ON LBAL.ParentId = LO.Id AND LBAL.Id = Bal.BalId
WHERE        (Bal.TypeId = 42)
GO
/****** Object:  Table [dbo].[Log]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [uniqueidentifier] NOT NULL,
	[ApplicationId] [bigint] NOT NULL,
	[User] [nvarchar](max) NULL,
	[dh] [datetime] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[LocationId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Route_View_Historycal]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_Route_View_Historycal]
AS
SELECT top (5000) dbo.[Log].LocationId, dbo.[Log].[User], dbo.[Log].Message, dbo.Location.Name, log.dh
FROM   dbo.[Log] INNER JOIN
             dbo.Location ON dbo.Location.Id = dbo.[Log].LocationId
WHERE (dbo.[Log].ApplicationId = 1)
ORDER BY dbo.[Log].dh DESC
GO
/****** Object:  Table [dbo].[rProductionRoute]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rProductionRoute](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductionId] [uniqueidentifier] NOT NULL,
	[RouteId] [bigint] NOT NULL,
	[dhi] [datetime] NULL,
	[dhf] [datetime] NULL,
	[Load] [float] NOT NULL,
	[InitialLoad] [float] NOT NULL,
	[Final] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[NWagon] [int] NOT NULL,
	[Dismember] [int] NOT NULL,
	[InitialNWagon] [int] NULL,
	[InstrumentId] [bigint] NULL,
	[Cleaning] [bit] NULL,
	[Percent] [float] NULL,
	[FinalRouteId] [bigint] NULL,
	[dhFinalRouteId] [datetime] NULL,
	[FinalLoad] [float] NULL,
	[Gate] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.rProductionRoute] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Production]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Production](
	[Id] [uniqueidentifier] NOT NULL,
	[dhi] [datetime] NULL,
	[dhf] [datetime] NULL,
	[Final] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Production] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rProductionStock]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rProductionStock](
	[Id] [uniqueidentifier] NOT NULL,
	[GoalI] [int] NOT NULL,
	[GoalF] [int] NOT NULL,
	[Load] [float] NOT NULL,
	[InitialLoad] [float] NULL,
	[InstrumentId] [bigint] NULL,
 CONSTRAINT [PK_dbo.rProductionStock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwRateioGpvDescPublish]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwRateioGpvDescPublish]
AS
SELECT        rProductionRoute.Id AS Id, Production.id AS [PassoId], GpvCode AS [CodRotaGpv], Production.dhi AS [PassoDhInicio], Production.dhf AS [PassoDhFim], Production.Final AS [PassoFinalizado], '' AS [BercoNome], 
                         D .Name AS [DestinoNome], rProductionStock. LOAD AS [DestinoTotal], O.Name AS [OrigemNome], rProductionRoute.dhi AS [OrigemDhInicio], rProductionRoute.dhf AS [OrigemDhFim], ROUND(isnull(FinalLoad, 0), 0) 
                         AS [OrigemTotal], rProductionRoute.NWagon AS NVagao, Bal.Name AS Balanca
FROM            Route.dbo.Production INNER JOIN
                         Route.dbo.rProductionStock ON Route.dbo.rProductionStock.Id = Production.Id INNER JOIN
                             (SELECT        rProductionRoute.Id, ProductionId, isnull(rProductionRoute.FinalRouteId, rProductionRoute.RouteId) AS RouteId, LOAD, FinalLoad, dhi, dhf, NWagon
                               FROM            Route.dbo.rProductionRoute ) AS rProductionRoute ON rProductionRoute.ProductionId = Production.Id INNER JOIN
                         Route.dbo.Location AS Bal ON Bal.Id = Route.dbo.rProductionStock.InstrumentId 
						 inner join Route on Route.Id = rProductionRoute.RouteId
						 INNER JOIN
                         Route.dbo.rRouteOxD ON rRouteOxD.Id = rProductionRoute.RouteId INNER JOIN
                         Route.dbo.OxD ON OxD.Id = rRouteOxD.OxDId INNER JOIN
                         Route.dbo.Location AS O ON OxD.OriginId = O.Id INNER JOIN
                         Route.dbo.Location AS D ON OxD.DestinationId = D .Id INNER JOIN
                             (SELECT        ProductionId, sum(LOAD) AS SumOr
                               FROM            [Route].[dbo].[rProductionRoute]
                               GROUP BY ProductionId) AS SumOr ON SumOr.ProductionId = Production.Id INNER JOIN
                             (SELECT        [ProductionId], CASE WHEN sum(NWagon) > 0 THEN max(rProductionStock. LOAD) / cast(isnull(sum(NWagon), 0) AS float) ELSE 0 END AS LoadAvg
                               FROM            [Route].[dbo].[rProductionRoute] INNER JOIN
                                                         route.dbo.rProductionStock ON rProductionStock.Id = rProductionRoute.ProductionId INNER JOIN
                                                         Route.dbo.rRouteOxD ON rRouteOxD.Id = rProductionRoute.RouteId INNER JOIN
                                                         Route.dbo.OxD ON OxD.Id = rRouteOxD.OxDId
                               GROUP BY ProductionId) AS T2 ON T2.ProductionId = rProductionRoute.ProductionId
WHERE        (rProductionStock. LOAD > 50) AND (Production.dhi > '2023-11-21 10:34:29.393') AND (rProductionRoute.NWagon > 0)
GO
/****** Object:  View [dbo].[vwRateioHistoryDescPublish]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[vwRateioHistoryDescPublish]
AS
SELECT        Id, ISNULL(OrigemDhInicio, GETDATE()) AS Inicio, DestinoNome AS Destino, Balanca AS Bal, DestinoTotal AS Total, CAST(ISNULL(NVagao, 0) AS float) AS Vagoes, OrigemTotal / (CASE WHEN NVagao = 0 OR
                         NVagao IS NULL THEN 1 ELSE NVagao END) AS TonToVag, CASE WHEN OrigemNome = 'VV01' THEN round(OrigemTotal, 0) ELSE 0 END AS VV01, CASE WHEN OrigemNome = 'VV02' THEN round(OrigemTotal, 0) 
                         ELSE 0 END AS VV02, CASE WHEN OrigemNome = 'VV03' THEN round(OrigemTotal, 0) ELSE 0 END AS VV03, CASE WHEN OrigemNome = 'VV04' THEN round(OrigemTotal, 0) ELSE 0 END AS VV04, 
                         CASE WHEN OrigemNome = 'VV05' THEN round(OrigemTotal, 0) ELSE 0 END AS VV05, CAST(0 AS float) AS RC01, CAST(0 AS float) AS RC02A, CAST(0 AS float) AS RC03, CAST(0 AS float) AS RC04, CAST(0 AS float) AS RC05, 
                         CAST(0 AS float) AS RCP7, CAST(0 AS float) AS RCP8, CAST(0 AS float) AS RCP9, CAST(0 AS float) AS ER01, CAST(0 AS float) AS ER02, CAST(0 AS float) AS ER03, CAST(0 AS float) AS COM, CAST(0 AS float) AS USI, CAST(0 AS float) 
                         AS US8, PassoFinalizado AS Final, ISNULL(OrigemDhFim, DATEADD(hour, 1, OrigemDhInicio)) AS Termino
FROM            dbo.vwRateioGpvDescPublish
WHERE        (DestinoTotal > 0)
GO
/****** Object:  Table [dbo].[rTagGroup]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rTagGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ParentId] [bigint] NULL,
	[TagId] [bigint] NULL,
	[OpcServer] [nvarchar](max) NULL,
	[AddrOpcServer] [nvarchar](max) NULL,
	[Url_Read] [nvarchar](max) NULL,
	[Url_Write] [nvarchar](max) NULL,
	[Rate] [float] NOT NULL,
	[WindowsService] [nvarchar](max) NULL,
	[SqlProcedure] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.rTagGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Tag_Media_Driver]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[vw_Tag_Media_Driver]
AS
SELECT dbo.rLocationValue.Id, Substring(l1.Name, 5, LEN(L1.NAME)) as nome, dbo.rLocationValue.ValueDateTime, dbo.rLocationValue.ValueInt, T1.dh, T1.LastDh, 
((cast (DATEPART(HOUR, rLocationValue.ValueDateTime) as float)*3600) + (cast (DATEPART(MINUTE, dbo.rLocationValue.ValueDateTime) as float) * 60) + cast (DATEPART(second, dbo.rLocationValue.ValueDateTime) as float)) / cast (dbo.rLocationValue.ValueInt as float) AS Media,
CASE WHEN ((cast (DATEPART(HOUR, rLocationValue.ValueDateTime) as float)*3600) + (cast (DATEPART(MINUTE, dbo.rLocationValue.ValueDateTime) as float) * 60) + cast (DATEPART(second, dbo.rLocationValue.ValueDateTime) as float)) / cast (dbo.rLocationValue.ValueInt as float) > 7 or DATEDIFF(second, dh, getdate()) > 60 THEN 'Bad' ELSE 'Good' END AS Expr1
FROM   dbo.rLocationValue INNER JOIN
                 (SELECT MAX(nome.dh) AS dh, MIN(nome.LastDh) AS LastDh, nome.Id
                 FROM    dbo.rLocationValue AS rLocationValue_2 
				 INNER JOIN
                                  (SELECT DISTINCT rLocationValue_1.Id, rLocationValue_1.ValueInt, rLocationValue_1.ValueDateTime, meas.dh, meas.LastDh
                                  FROM    dbo.rLocationValue AS rLocationValue_1 INNER JOIN
                                               dbo.Location AS LValue ON LValue.Id = rLocationValue_1.Id INNER JOIN
                                               dbo.Location AS Plc ON Plc.Id = LValue.ParentId INNER JOIN
                                               dbo.Tag ON dbo.Tag.PlcId = Plc.Id INNER JOIN
                                               dbo.rTagGroup AS tg ON tg.TagId = dbo.Tag.Id INNER JOIN
                                               dbo.rInstrumentMeasure AS meas ON meas.TagId = dbo.Tag.Id
                                  GROUP BY rLocationValue_1.Id, rLocationValue_1.ValueInt, rLocationValue_1.ValueDateTime, meas.dh, meas.LastDh) AS nome ON nome.Id = rLocationValue_2.Id
                 GROUP BY nome.Id) AS T1 ON T1.Id = dbo.rLocationValue.Id
				 inner join Location as l1 on l1.Id = rLocationValue.Id
GO
/****** Object:  View [dbo].[vw_Route_Tag_Feeder_Command]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Tag_Feeder_Command]
AS
SELECT        TAG_OK.Id AS TagId, vw.Id, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, MEAS_OK.Value AS TagValue, REFVAL.Value AS Reference, 'Feeder command' AS Type, CM_L1.Name
FROM            dbo.vw_Route_Consistency_Feeder AS vw INNER JOIN
                         dbo.Location AS CM ON CM.Id = vw.FeederId INNER JOIN
                         dbo.Location AS CM_L1 ON CM_L1.ParentId = CM.Id INNER JOIN
                         dbo.Command AS CMD ON CMD.Id = CM_L1.Id INNER JOIN
                         dbo.Location AS CMD_REF ON CMD_REF.ParentId = CMD.Id INNER JOIN
                         dbo.Reference AS REFVAL ON REFVAL.Id = CMD_REF.Id AND REFVAL.LocReferenceId = vw.AssetNextId INNER JOIN
                         dbo.Location AS CMD_INS ON CMD_INS.ParentId = REFVAL.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = CMD_INS.Id INNER JOIN
                         dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id LEFT OUTER JOIN
                         dbo.vw_Eqp_RouteActive AS EqpAct ON EqpAct.Id = vw.AssetCurrentId
WHERE        (EqpAct.Id IS NULL)
GO
/****** Object:  View [dbo].[vw_Route_Rule_OldArea]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[vw_Route_Rule_OldArea]
AS
SELECT TOP (1000) rra.Id, dbo.Location.Name, rrgs.[Order], loc.Name AS Expr1,
                 (SELECT CASE WHEN MAX(CASE WHEN name LIKE '%rc01%' THEN 1 ELSE 0 END) = 1 AND MAX(CASE WHEN name LIKE '%ep01%' THEN 1 ELSE 0 END) = 1 THEN 'Sim' ELSE 'Nao' END AS Expr1
                 FROM    dbo.rRouteActive INNER JOIN
                              dbo.Location AS l1 ON l1.Id = dbo.rRouteActive.Id
                 WHERE (l1.Name LIKE '%rc01%') OR
                              (l1.Name LIKE '%ep01%')) AS TRA02,
                 (SELECT CASE WHEN MAX(CASE WHEN name LIKE '%RC02A%' THEN 1 ELSE 0 END) = 1 AND MAX(CASE WHEN name LIKE '%EP02A%' THEN 1 ELSE 0 END) = 1 THEN 'Sim' ELSE 'Nao' END AS Expr1
                 FROM    dbo.rRouteActive AS rRouteActive_1 INNER JOIN
                              dbo.Location AS l1 ON l1.Id = rRouteActive_1.Id
                 WHERE (l1.Name LIKE '%RC02A%') OR
                              (l1.Name LIKE '%EP02A%')) AS TRB03A
FROM   dbo.rRouteActive AS rra INNER JOIN
             dbo.Location ON dbo.Location.Id = rra.Id INNER JOIN
             dbo.rRouteGraphLocation AS rrgl ON rrgl.LocationId = rra.Id INNER JOIN
             dbo.rRouteGraphSequence AS rrgs ON rrgs.RouteGraphId = rrgl.Id INNER JOIN
             dbo.Location AS loc ON loc.Id = rrgs.LocationId
WHERE (loc.Name LIKE '%RC01%') OR
             (loc.Name LIKE '%EP01%') OR
             (loc.Name LIKE '%RC02%') OR
             (loc.Name LIKE '%EP02%')
GROUP BY rra.Id, dbo.Location.Name, rrgs.[Order], loc.Name
GO
/****** Object:  View [dbo].[vw_Eqp_FinalProduction]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Eqp_FinalProduction]
AS
SELECT        Eqp.Name AS Asset, Eqp.Id AS EqpId, Meas.TagId, dbo.Tag.Name AS Tag, Meas.Value, Meas.LastValue, Meas.Id AS InstrumentId
FROM            dbo.Location AS Eqp INNER JOIN
                         dbo.Location AS L1 ON Eqp.Id = L1.ParentId AND L1.TypeId = 43 INNER JOIN
                         dbo.Location AS L2 ON L1.Id = L2.ParentId INNER JOIN
                         dbo.rInstrumentMeasure AS Meas ON Meas.Id = L2.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = Meas.TagId
GO
/****** Object:  Table [dbo].[Plc]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plc](
	[Id] [bigint] NOT NULL,
	[Resource] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Plc] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Tag_Media_Driver_Optimized]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Tag_Media_Driver_Optimized]
AS
WITH DriverMetrics AS (
    -- 1. Calcula as métricas de tempo e contagem por Driver
    SELECT
        rLV.Id,
        rLV.ValueDateTime AS TimeAccumulated, -- Tempo acumulado de execução dos ciclos
        rLV.ValueInt AS CycleCount,          -- Número de ciclos de execução
        L.Name AS LocationName
    FROM
        dbo.rLocationValue rLV
    INNER JOIN
        dbo.Location L ON L.Id = rLV.Id -- Assumindo que rLocationValue.Id é o ID do Driver (LocationId)
),
TagHealth AS (
    -- 2. Encontra o 'Tag' mais recente/antigo para cada Driver
    SELECT
        rTG.Id AS DriverId,
        MAX(rIM.dh) AS LastTagRead,          -- Último dh (leitura)
        MIN(rIM.LastDh) AS OldestTagLastDh   -- Último LastDh (para referência)
    FROM
        dbo.rTagGroup rTG
    INNER JOIN
        dbo.Tag T ON T.Id = rTG.TagId
    INNER JOIN
        dbo.Plc P ON P.Id = T.PlcId
    INNER JOIN
        dbo.rInstrumentMeasure rIM ON rIM.TagId = T.Id
    -- Filtra por Drivers Parent (que são as instâncias)
    WHERE rTG.ParentId IS NULL OR rTG.ParentId = 0 
    GROUP BY rTG.Id
)
-- 3. Combina as métricas e aplica as regras de saúde
SELECT
    M.Id,
    SUBSTRING(M.LocationName, 5, LEN(M.LocationName)) AS DriverName, -- Remove os 4 primeiros chars (Ex: 'DRV_')
    M.TimeAccumulated,
    M.CycleCount,
    H.LastTagRead,

    -- Cálculo da Média de Latência (em segundos)
    CAST(
        (
            (CAST(DATEPART(HOUR, M.TimeAccumulated) AS FLOAT) * 3600) +
            (CAST(DATEPART(MINUTE, M.TimeAccumulated) AS FLOAT) * 60) +
            (CAST(DATEPART(SECOND, M.TimeAccumulated) AS FLOAT))
        ) / 
        CAST(M.CycleCount AS FLOAT) 
    AS DECIMAL(10, 2)) AS AvgLatencySeconds,

    -- Cálculo do tempo de desatualização do dado (em segundos)
    DATEDIFF(second, H.LastTagRead, GETDATE()) AS StalenessSeconds,

    -- Status de Saúde (HealthStatus)
    CASE 
        WHEN 
            (
                (CAST(DATEPART(HOUR, M.TimeAccumulated) AS FLOAT) * 3600) + 
                (CAST(DATEPART(MINUTE, M.TimeAccumulated) AS FLOAT) * 60) + 
                (CAST(DATEPART(SECOND, M.TimeAccumulated) AS FLOAT))
            ) / CAST(M.CycleCount AS FLOAT) > 7 
            OR DATEDIFF(second, H.LastTagRead, GETDATE()) > 60 
        THEN 'Bad' 
        ELSE 'Good' 
    END AS HealthStatus

FROM
    DriverMetrics M
INNER JOIN
    TagHealth H ON H.DriverId = M.Id
GO
/****** Object:  Table [dbo].[Instrument]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrument](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Instrument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_Tag_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Tag_Permission]
AS
SELECT DISTINCT dbo.Tag.Id AS TagId, '[' + PLC.Alias + ']' + dbo.Tag.Name AS Tag, 'Tags Permission' AS Type
FROM            dbo.Location AS LP INNER JOIN
                         dbo.Location AS L1 ON L1.ParentId = LP.Id AND LP.TypeId = 9 LEFT OUTER JOIN
                         dbo.Location AS L2 ON L2.ParentId = L1.Id INNER JOIN
                         dbo.Instrument AS I ON I.Id = CASE WHEN L2.Id IS NULL THEN L1.Id ELSE L2.Id END INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS ON MEAS.Id = I.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = MEAS.TagId INNER JOIN
                         dbo.Location AS PLC ON dbo.Tag.PlcId = PLC.Id
WHERE        (dbo.Tag.Name NOT LIKE '%_UNLATCH%')
GO
/****** Object:  View [dbo].[vw_Tag_Permission_Deactivate]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Script for SelectTopNRows Permission from SSMS  *****
AND (VW_RV.Id IS NULL)
***** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[vw_Tag_Permission_Deactivate]
AS
SELECT        vw.TagId, vw.Tag, vw.Type
FROM            dbo.vw_Tag_Permission AS vw LEFT OUTER JOIN
                             (SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Damper_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Eqp_O_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Eqp_D_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Eqp_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Feeder_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_OxD_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Reversal_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_Tag_Tripper_Permission
                               UNION
                               SELECT        TagId
                               FROM            dbo.vw_Route_VV_Limp_Permission) AS Act ON Act.TagId = vw.TagId
WHERE        (Act.TagId IS NULL)
GO
/****** Object:  Table [dbo].[Origin]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Origin](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Origin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_RouteOxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_RouteOxD]
AS
SELECT        rgl.LocationId AS Id, rgmn.LocationId AS OriginId, rgmx.LocationId AS DestinationId, r.Name AS Route
FROM            (SELECT        dbo.rRouteGraphSequence.RouteGraphId, COUNT(*) AS ct, CASE WHEN (STRING_AGG(Name, ' ') LIKE '%USI%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%ITB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%US8%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%HIS%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%KOB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%NIB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%HIS%') AND STRING_AGG(Name, ' ') LIKE '%RC%' THEN 'Eqp' WHEN (STRING_AGG(Name, ' ') LIKE '%USI%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%ITB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%US8%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%HIS%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%KOB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%NIB%' OR
                                                    STRING_AGG(Name, ' ') LIKE '%HIS%') AND STRING_AGG(Name, ' ') NOT LIKE '%RC%' THEN 'Usi' ELSE 'Eqp' END AS Type
                          FROM            dbo.rRouteGraphSequence INNER JOIN
                                                    dbo.Origin ON dbo.Origin.Id = dbo.rRouteGraphSequence.LocationId INNER JOIN
                                                    dbo.Location ON dbo.Location.Id = dbo.rRouteGraphSequence.LocationId
                          WHERE        (dbo.rRouteGraphSequence.[Order] < 3)
                          GROUP BY dbo.rRouteGraphSequence.RouteGraphId) AS cto INNER JOIN
                         dbo.rRouteGraphSequence AS rgmn ON rgmn.RouteGraphId = cto.RouteGraphId INNER JOIN
                         dbo.Location AS Location_1 ON Location_1.Id = rgmn.LocationId INNER JOIN
                         dbo.Origin AS Origin_1 ON Origin_1.Id = Location_1.Id INNER JOIN
                             (SELECT        RouteGraphId, MAX([Order]) AS mx, MIN([Order]) AS mn
                               FROM            dbo.rRouteGraphSequence AS rRouteGraphSequence_1
                               GROUP BY RouteGraphId) AS mmr ON mmr.RouteGraphId = cto.RouteGraphId AND mmr.mx <> rgmn.[Order] INNER JOIN
                         dbo.rRouteGraphLocation AS rgl ON rgl.Id = mmr.RouteGraphId INNER JOIN
                         dbo.rRouteGraphSequence AS rgmx ON rgmx.RouteGraphId = cto.RouteGraphId AND rgmx.[Order] = mmr.mx INNER JOIN
                         dbo.rRouteActive AS act ON act.Id = rgl.LocationId INNER JOIN
                         dbo.Location AS r ON r.Id = act.Id
WHERE        (rgmn.[Order] = 1) AND (cto.ct > 1) AND (cto.Type = 'Usi') OR
                         (rgmn.[Order] = 2) AND (cto.Type = 'Eqp') OR
                         (cto.ct = 1)
GO
/****** Object:  View [dbo].[vw_Tag_Overturned_Wagons_Instrument]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Tag_Overturned_Wagons_Instrument]
AS
SELECT        Eqp.Name AS Location, Eqp.Id AS LocationId, '[' + Plc.Name + ']' + dbo.Tag.Name AS Tag, InsVal.TagId, Plc.Name AS Plc, Plc.Id AS PlcId, InsVal.Id AS InstrumentId, InsVal.Value
FROM            dbo.Location AS L1 INNER JOIN
                         dbo.Location AS L2 ON L2.ParentId = L1.Id AND L1.TypeId = 44 INNER JOIN
                         dbo.Location AS Eqp ON L1.ParentId = Eqp.Id INNER JOIN
                         dbo.rInstrumentMeasure AS InsVal ON InsVal.Id = L2.Id INNER JOIN
                         dbo.Tag ON dbo.Tag.Id = InsVal.TagId INNER JOIN
                         dbo.Location AS Plc ON Plc.Id = dbo.Tag.PlcId
GO
/****** Object:  View [dbo].[vw_Queue_OxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Queue_OxD]
AS
SELECT        L1.Id, TAG_OK.Id AS TagId, '[' + PLC.Alias + ']' + TAG_OK.Name AS Tag, dbo.rLocationValue.ValueFloat AS Idx, 'Fila Sistema OxD' AS Type, L1.Name AS Position
FROM            dbo.rLocationValue INNER JOIN
                         dbo.Location AS L1 ON L1.Id = dbo.rLocationValue.Id AND L1.TypeId = 54 INNER JOIN
                         dbo.Location AS L2 ON L2.ParentId = L1.Id INNER JOIN
                         dbo.rInstrumentMeasure AS MEAS_OK ON MEAS_OK.Id = L2.Id INNER JOIN
                         dbo.Tag AS TAG_OK ON TAG_OK.Id = MEAS_OK.TagId INNER JOIN
                         dbo.Location AS PLC ON TAG_OK.PlcId = PLC.Id
GO
/****** Object:  View [dbo].[vw_Queue_OxD_Route]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Queue_OxD_Route]
AS
SELECT        vw.Id, vw.Position, Route.ActiveId AS RouteId, vw.Idx
FROM            dbo.vw_Queue_OxD AS vw LEFT OUTER JOIN
                             (SELECT        LVal.Id AS LocationId, LVal.ValueFloat, dbo.rRouteOxD.OxDId, dbo.rRouteActive.Id AS ActiveId
                               FROM            dbo.rRouteActive INNER JOIN
                                                         dbo.rRouteOxD ON dbo.rRouteOxD.LocationId = dbo.rRouteActive.Id INNER JOIN
                                                         dbo.Location AS OxD ON OxD.Id = dbo.rRouteOxD.OxDId INNER JOIN
                                                         dbo.Location AS L1 ON L1.ParentId = OxD.Id AND L1.TypeId = 52 INNER JOIN
                                                         dbo.rLocationValue AS LVal ON LVal.Id = L1.Id) AS Route ON Route.ValueFloat = vw.Idx
GO
/****** Object:  View [dbo].[vw_Production_Rateio]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Production_Rateio]
AS
SELECT      
R.id AS ProductionRouteId, 
R.ProductionId, SumOrigin, 
rProductionStock.Load as DestinationLoadTotal,
CASE WHEN SumOrigin > 0 THEN r.LOAD / cast(SumOrigin as float) ELSE 0 END AS PercTotal,
rProductionStock.Load - isnull(SumFinal,0) as DestinationLoadCurr,
CASE 
WHEN isnull(SumCurr,0) > 0 THEN isnull(LoadOriginCurr,0)/isnull(cast(SumCurr as float),0) else 0 end as PercCurr,
(rProductionStock.Load - isnull(SumFinal,0))*(CASE 
WHEN isnull(SumCurr,0) > 0 THEN isnull(LoadOriginCurr,0)/isnull(cast(SumCurr as float),0)else 0 end) as DestinationLoadCalc
FROM            Production 
inner join rProductionStock on rProductionStock.Id = Production.Id
INNER JOIN [Route].[dbo].[rProductionRoute] AS R ON R.ProductionId = Production.Id 
INNER JOIN
                             (SELECT        ProductionId, sum(LOAD) AS SumOrigin
                               FROM            [Route].[dbo].[rProductionRoute]
                               GROUP BY ProductionId) AS TA ON Production.Id = TA.ProductionId
inner join
                             (SELECT        Load as LoadOriginCurr, Id
                               FROM            [Route].[dbo].[rProductionRoute]
							   where
							   final = 0 or Cleaning = 1
                              ) AS TB ON R.Id = TB.Id
Left outer join
                             (SELECT        sum(Load) as SumCurr, ProductionId
                               FROM            [Route].[dbo].[rProductionRoute]
							   where
							   final = 0 or Cleaning = 1
							   GROUP BY ProductionId
                              ) AS TC ON Production.Id = TC.ProductionId
left outer join
                             (SELECT        ProductionId, isnull(sum(FinalLoad),0) AS SumFinal
                               FROM            [Route].[dbo].[rProductionRoute]
							   where
							   final = 1 and Cleaning = 0
                               GROUP BY ProductionId) AS TD ON Production.Id = TD.ProductionId
where
Production.final = 0 and DATEDIFF(day,R.dhi ,getdate()) < 1

GO
/****** Object:  View [dbo].[vw_Route_Consistency]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Consistency]
AS
SELECT        Q.Id, MIN(CASE WHEN tableoftrue.dumper = 0 OR
                         tableoftrue.feeder = 0 OR
                         tableoftrue.reversal = 0 OR
                         tableoftrue.[rule] = 0 OR
                         tableoftrue.tripper = 0 THEN 0 ELSE 1 END) AS Consistency
FROM            dbo.rRouteQueue AS Q LEFT OUTER JOIN
                             (SELECT        Queu.Id, ISNULL(D.BoolVeredict, 1) AS dumper, ISNULL(R.BoolVeredict, 1) AS [rule], ISNULL(F.BoolVeredict, 1) AS feeder, ISNULL(T.BoolVeredict, 1) AS tripper, ISNULL(Rv.BoolVeredict, 1) AS reversal
                               FROM            dbo.rRouteQueue AS Queu LEFT OUTER JOIN
                                                         dbo.vw_Route_Consistency_Damper AS D ON D.Id = Queu.Id LEFT OUTER JOIN
                                                         dbo.vw_Route_Consistency_Rule AS R ON R.Id = Queu.Id LEFT OUTER JOIN
                                                         dbo.vw_Route_Consistency_Feeder AS F ON F.Id = Queu.Id LEFT OUTER JOIN
                                                         dbo.vw_Route_Consistency_Tripper AS T ON T.Id = Queu.Id LEFT OUTER JOIN
                                                         dbo.vw_Route_Consistency_Reversal AS Rv ON Rv.Id = Queu.Id) AS tableoftrue ON tableoftrue.Id = Q.Id
GROUP BY Q.Id
GO
/****** Object:  View [dbo].[vw_Route_Log]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Route_Log]
AS
SELECT TOP (20) dbo.[Log].Id, dbo.[Log].ApplicationId, dbo.[Log].[User], dbo.[Log].dh, dbo.[Log].Message, dbo.[Log].LocationId, dbo.Location.Name AS Rota
FROM   dbo.[Log] INNER JOIN
             dbo.Location ON dbo.Location.Id = dbo.[Log].LocationId
WHERE (dbo.[Log].dh > DATEADD(day, - 1, GETDATE())) AND (dbo.[Log].ApplicationId = 1) AND (dbo.[Log].Message <> '')
ORDER BY dbo.[Log].dh DESC
GO
/****** Object:  View [dbo].[vw_Tag_Dismember_Instrument]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Tag_Dismember_Instrument]
AS
SELECT        dbo.Location.Id AS LocationId, L2.Id AS InstrumentId
FROM            dbo.Location INNER JOIN
                         dbo.Location AS L1 ON L1.ParentId = dbo.Location.Id INNER JOIN
                         dbo.Location AS L2 ON L2.ParentId = L1.Id
WHERE        (L1.TypeId = 48)
GO
/****** Object:  View [dbo].[vw_RouteLog]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_RouteLog]
AS
SELECT TOP (100) PERCENT NEWID() AS Id, dbo.[Log].dh AS 'Horário', dbo.[Log].Message AS Evento, USER AS Computador, dbo.Location.Name AS Rota
FROM     dbo.[Log] INNER JOIN
                  dbo.Location ON dbo.Location.Id = dbo.[Log].LocationId
WHERE  (dbo.[Log].dh > DATEADD(day, - 2, GETDATE()))
ORDER BY 'Horário' DESC
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Active]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Active](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Active] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Application] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Berth]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Berth](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Berth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Boarding]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Boarding](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Boarding] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CheckList]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CheckList](
	[Id] [uniqueidentifier] NOT NULL,
	[dh] [datetime] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[Yes] [int] NOT NULL,
	[No] [int] NOT NULL,
	[NA] [int] NOT NULL,
	[Max] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[User] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.CheckList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compartment]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compartment](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Compartment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conveyor]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conveyor](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Conveyor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Destination](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Destination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flow]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flow](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Flow] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Graph]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Graph](
	[Id] [bigint] NOT NULL,
	[LocationAId] [bigint] NOT NULL,
	[LocationBId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Graph] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[History]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[History](
	[Id] [uniqueidentifier] NOT NULL,
	[dh] [datetime] NOT NULL,
	[Action] [nvarchar](max) NULL,
	[User] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.History] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Information]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Information](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Information] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Line]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Line](
	[Id] [bigint] NOT NULL,
	[OriginId] [bigint] NULL,
	[DestinationId] [bigint] NULL,
 CONSTRAINT [PK_dbo.Line] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Load]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Load](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Load] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logbook]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logbook](
	[Id] [uniqueidentifier] NOT NULL,
	[LocationId] [bigint] NULL,
	[dh] [datetime] NULL,
	[User] [nvarchar](max) NULL,
	[AreaSelected] [nvarchar](max) NULL,
	[Observation] [nvarchar](max) NULL,
	[LandmarkInitial] [int] NULL,
	[LandmarkFinal] [int] NULL,
	[WeightAsset] [int] NULL,
	[WeightConjunct] [int] NULL,
	[ShipCradle] [int] NULL,
	[Hold] [int] NULL,
	[Cod] [nvarchar](max) NULL,
	[Material] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
	[FlowMeasured] [int] NULL,
	[FlowReal] [int] NULL,
	[isFail] [bit] NULL,
	[hi] [int] NULL,
	[mi] [int] NULL,
	[hf] [int] NULL,
	[mf] [int] NULL,
	[Asset] [nvarchar](max) NULL,
	[Conjunct] [nvarchar](max) NULL,
	[MaterialQuality] [bit] NULL,
	[RestrictRoute] [bit] NULL,
	[StackEnd] [bit] NULL,
	[LmInitialNorthDirection] [bit] NULL,
	[LmFinalNorthDirection] [bit] NULL,
	[IsHopper] [bit] NULL,
	[LowFlowOrParalization] [bit] NULL,
	[LotNumber] [int] NULL,
	[NumberOfWagons] [int] NULL,
	[LotPrefix] [int] NULL,
	[Steps] [int] NULL,
	[LotSLandmarkInitial] [int] NULL,
	[LotSLandmarkFinal] [int] NULL,
	[hP0] [int] NULL,
	[mP0] [int] NULL,
	[hUsina] [int] NULL,
	[mUsina] [int] NULL,
	[hAcoplado] [int] NULL,
	[mAcoplado] [int] NULL,
	[FlowObservation] [nvarchar](max) NULL,
	[StackingWay] [nvarchar](max) NULL,
	[OutOfStack] [nvarchar](max) NULL,
	[User2] [nvarchar](max) NULL,
	[RestrictVV] [bit] NULL,
 CONSTRAINT [PK_dbo.Logbook] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MigraOxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MigraOxD](
	[id] [int] NULL,
	[OxDid] [int] NULL,
	[IdxO] [int] NULL,
	[IdxD] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Position]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Position] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorCapacity]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorCapacity](
	[Id] [bigint] NOT NULL,
	[RollerTilt] [float] NOT NULL,
	[Width] [float] NOT NULL,
	[ConveyorIncline] [float] NOT NULL,
	[Capacity] [float] NOT NULL,
 CONSTRAINT [PK_dbo.rConveyorCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorConsumption]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorConsumption](
	[Id] [bigint] NOT NULL,
	[Consumption] [float] NOT NULL,
 CONSTRAINT [PK_dbo.rConveyorConsumption] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorInputPosition]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorInputPosition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Position] [float] NOT NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_dbo.rConveyorInputPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorLenght]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorLenght](
	[Id] [bigint] NOT NULL,
	[Lenght] [float] NOT NULL,
 CONSTRAINT [PK_dbo.rConveyorLenght] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorOutputPosition]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorOutputPosition](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Position] [float] NOT NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_dbo.rConveyorOutputPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorScale]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorScale](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Position] [float] NOT NULL,
	[LocationId] [bigint] NULL,
 CONSTRAINT [PK_dbo.rConveyorScale] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rConveyorSpeed]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rConveyorSpeed](
	[Id] [bigint] NOT NULL,
	[Speed] [float] NOT NULL,
 CONSTRAINT [PK_dbo.rConveyorSpeed] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resource]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resource](
	[Id] [uniqueidentifier] NOT NULL,
	[TypeId] [bigint] NULL,
	[dhInitial] [datetime] NOT NULL,
	[dhCommunicated] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Quantity] [int] NOT NULL,
	[Completed] [bit] NOT NULL,
	[Recused] [bit] NOT NULL,
	[Response] [nvarchar](max) NULL,
	[Requester] [nvarchar](max) NULL,
	[Location] [nvarchar](max) NULL,
	[Finish] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Resource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLocationDataHistory]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLocationDataHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[LocationId] [bigint] NULL,
	[dh] [datetime] NOT NULL,
	[Flow] [float] NULL,
	[Material] [nvarchar](max) NULL,
	[Operador] [nvarchar](max) NULL,
	[InitialMark] [int] NULL,
	[FinalMark] [int] NULL,
	[Total] [float] NULL,
	[OperationTime] [int] NULL,
 CONSTRAINT [PK_dbo.rLocationDataHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLocationEnable]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLocationEnable](
	[Id] [bigint] NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.rLocationEnable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLocationHistory]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLocationHistory](
	[Id] [bigint] NOT NULL,
	[Resource] [nvarchar](max) NULL,
	[HistoryId] [uniqueidentifier] NOT NULL,
	[Location_Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rLocationHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLogbookEPEE]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLogbookEPEE](
	[Id] [uniqueidentifier] NOT NULL,
	[In_Diario_Origem1] [nvarchar](max) NULL,
	[In_Diario_Matricula] [nvarchar](max) NULL,
	[In_Diario_Material1] [nvarchar](max) NULL,
	[In_Diario_Fechamento_de_Pilha] [bit] NOT NULL,
	[In_Diario_Numero_Lote] [int] NOT NULL,
	[In_Diario_Quantidade_Vagoes1] [int] NOT NULL,
	[In_Diario_Prefixo_Lote1] [nvarchar](max) NULL,
	[In_Diario_Passadas] [int] NOT NULL,
	[In_Diario_Baliza_Inicial] [int] NOT NULL,
	[In_Diario_Baliza_Final] [int] NOT NULL,
	[In_Diario_Baliza_Inicial_Lote] [int] NOT NULL,
	[In_Diario_Baliza_Final_Lote] [int] NOT NULL,
	[In_Diario_Hora_Inicial1] [int] NOT NULL,
	[In_Diario_Minuto_Inicial1] [int] NOT NULL,
	[In_Diario_Hora_Final1] [int] NOT NULL,
	[In_Diario_Minuto_Final1] [int] NOT NULL,
	[In_Diario_Paralisacao] [bit] NOT NULL,
	[In_Diario_Observacao_Paralisacao] [nvarchar](max) NULL,
	[In_Diario_Observacao_Taxa] [nvarchar](max) NULL,
	[In_Diario_Tremonha] [bit] NOT NULL,
	[In_Diario_Qualidade_Material_Ok] [bit] NOT NULL,
	[In_Diario_Rota_Restrita_Ok] [bit] NOT NULL,
	[In_Diario_Codigo_Operacao] [nvarchar](max) NULL,
	[In_Diario_Area] [nvarchar](max) NULL,
	[In_Diario_Empilhamento] [nvarchar](max) NULL,
	[In_Diario_Insumo] [nvarchar](max) NULL,
	[In_Diario_Hora_Comunicacao_P0] [int] NOT NULL,
	[In_Diario_Minuto_Comunicacao_P0] [int] NOT NULL,
	[In_Diario_Hora_Acoplado] [int] NOT NULL,
	[In_Diario_Minuto_Acoplado] [int] NOT NULL,
	[In_Diario_Hora_Rodou_Usina] [int] NOT NULL,
	[In_Diario_Minuto_Rodou_Usina] [int] NOT NULL,
	[In_Diario_Empilhadeira] [nvarchar](max) NULL,
	[In_Diario_Material2] [nvarchar](max) NULL,
	[In_Diario_Origem2] [nvarchar](max) NULL,
	[In_Diario_Taxa_1] [int] NOT NULL,
	[In_Diario_Taxa_2] [int] NOT NULL,
	[In_Diario_Qualidade_Material_Nok] [bit] NOT NULL,
	[In_Diario_Rota_Restrita_Nok] [bit] NOT NULL,
	[In_Diario_Prefixo_Lote2] [nvarchar](max) NULL,
	[In_Diario_Quantidade_Vagoes2] [int] NOT NULL,
	[In_Diario_Hora_Inicial2] [int] NOT NULL,
	[In_Diario_Minuto_Inicial2] [int] NOT NULL,
	[In_Diario_Hora_Final2] [int] NOT NULL,
	[In_Diario_Minuto_Final2] [int] NOT NULL,
	[In_Diario_Baliza_Inicial_Lote_Direcao] [bit] NOT NULL,
	[In_Diario_Baliza_Final_Lote_Direcao] [bit] NOT NULL,
	[In_Diario_Restricao_Qualidade_Material] [int] NOT NULL,
	[In_Diario_Restricao_Rota_Restrita] [int] NOT NULL,
	[In_Diario_Peso_Lote1] [int] NOT NULL,
	[In_Diario_Peso_Lote2] [int] NOT NULL,
 CONSTRAINT [PK_dbo.rLogbookEPEE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLogbookHistory]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLogbookHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[LogbookId] [uniqueidentifier] NOT NULL,
	[HistoryId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.rLogbookHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rLogbookVV]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rLogbookVV](
	[Id] [uniqueidentifier] NOT NULL,
	[In_Diario_NomeOperador1] [nvarchar](max) NULL,
	[In_Diario_NomeOperador2] [nvarchar](max) NULL,
	[In_Diario_Observacao] [nvarchar](max) NULL,
	[In_Diario_ObservacaoTaxa] [nvarchar](max) NULL,
	[In_Diario_VVRestrito] [bit] NOT NULL,
	[In_Diario_RotaRestrita] [bit] NOT NULL,
	[In_Diario_QualidadeMaterial] [bit] NOT NULL,
	[In_Diario_PartirCicloHora] [int] NOT NULL,
	[In_Diario_PartirCicloMinuto] [int] NOT NULL,
	[In_Diario_TerminoDescargaHora] [int] NOT NULL,
	[In_Diario_TerminoDescargaMinuto] [int] NOT NULL,
	[In_Diario_BalizaMenor] [int] NOT NULL,
	[In_Diario_BalizaMaior] [int] NOT NULL,
	[In_Diario_QuantidadeVagoes] [int] NOT NULL,
	[In_Diario_Prefixo] [nvarchar](max) NULL,
	[In_Diario_Material] [nvarchar](max) NULL,
	[In_Diario_Area] [nvarchar](max) NULL,
	[In_Diario_Destino] [nvarchar](max) NULL,
	[In_Diario_Operador1Hora] [int] NOT NULL,
	[In_Diario_Operador1Minuto] [int] NOT NULL,
	[In_Diario_Operador2Hora] [int] NOT NULL,
	[In_Diario_Operador2Minuto] [int] NOT NULL,
	[In_Diario_Operador1QtVagoes] [int] NOT NULL,
 CONSTRAINT [PK_dbo.rLogbookVV] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rProductionBoarding]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rProductionBoarding](
	[Id] [uniqueidentifier] NOT NULL,
	[BerthId] [bigint] NOT NULL,
	[Compartment] [int] NOT NULL,
	[Load] [float] NOT NULL,
	[Current] [float] NOT NULL,
	[InitialLoad] [float] NOT NULL,
	[InstrumentId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rProductionBoarding] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rProductionOrigin]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rProductionOrigin](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductionId] [uniqueidentifier] NOT NULL,
	[GoalI] [int] NOT NULL,
	[GoalF] [int] NOT NULL,
 CONSTRAINT [PK_dbo.rProductionOrigin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteGraphGpv]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteGraphGpv](
	[Id] [bigint] NOT NULL,
	[GpvCode] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteGraphGpv] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteReplace]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteReplace](
	[Id] [bigint] NOT NULL,
	[LocationId] [bigint] NOT NULL,
	[dh] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteReplace] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRouteTravel]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRouteTravel](
	[Id] [bigint] NOT NULL,
	[Constant] [float] NOT NULL,
	[Variable] [float] NOT NULL,
	[GoalI] [float] NOT NULL,
	[GoalF] [float] NOT NULL,
 CONSTRAINT [PK_dbo.rRouteTravel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rRuleNavbar]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rRuleNavbar](
	[Id] [bigint] NOT NULL,
	[NameOption] [nvarchar](max) NULL,
	[Controller] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[ImageClass] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.rRuleNavbar] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rTagConveyorClean]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rTagConveyorClean](
	[Id] [bigint] NOT NULL,
	[ConveyorId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rTagConveyorClean] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rTagInformation]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rTagInformation](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.rTagInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rTagWrite]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rTagWrite](
	[Id] [bigint] NOT NULL,
	[Write] [bit] NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.rTagWrite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[GroupDns] [nvarchar](max) NULL,
	[ApplicationId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Rule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Step]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Step](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Step] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[Id] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.Stock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Type]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Type](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[rInstrumentMeasure] ADD  DEFAULT ((0)) FOR [Write]
GO
ALTER TABLE [dbo].[Route] ADD  DEFAULT ((0)) FOR [Inactive]
GO
ALTER TABLE [dbo].[rProductionRoute] ADD  CONSTRAINT [DF__rProducti__Clean__1E8F7FEF]  DEFAULT ((0)) FOR [Cleaning]
GO
ALTER TABLE [dbo].[rRouteGraphSequence] ADD  CONSTRAINT [DF__rRouteGra__Route__1E256B9B]  DEFAULT ((0)) FOR [RouteGraphId]
GO
ALTER TABLE [dbo].[rRouteOxD] ADD  CONSTRAINT [DF__rRouteOxD__Locat__22800C64]  DEFAULT ((0)) FOR [LocationId]
GO
ALTER TABLE [dbo].[Active]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Active_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Active] CHECK CONSTRAINT [FK_dbo.Active_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Berth]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Berth_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Berth] CHECK CONSTRAINT [FK_dbo.Berth_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Boarding]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Boarding_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Boarding] CHECK CONSTRAINT [FK_dbo.Boarding_dbo.Location_Id]
GO
ALTER TABLE [dbo].[CheckList]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CheckList_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CheckList] CHECK CONSTRAINT [FK_dbo.CheckList_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[Command]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Command_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Command] CHECK CONSTRAINT [FK_dbo.Command_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Compartment]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Compartment_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Compartment] CHECK CONSTRAINT [FK_dbo.Compartment_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Consistency]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Consistency_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Consistency] CHECK CONSTRAINT [FK_dbo.Consistency_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Conveyor]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Conveyor_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Conveyor] CHECK CONSTRAINT [FK_dbo.Conveyor_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Damper]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Damper_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Damper] CHECK CONSTRAINT [FK_dbo.Damper_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Destination]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Destination_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Destination] CHECK CONSTRAINT [FK_dbo.Destination_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Feeder]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Feeder_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Feeder] CHECK CONSTRAINT [FK_dbo.Feeder_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Flow]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Flow_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Flow] CHECK CONSTRAINT [FK_dbo.Flow_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Graph]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Graph_dbo.Location_LocationAId] FOREIGN KEY([LocationAId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Graph] CHECK CONSTRAINT [FK_dbo.Graph_dbo.Location_LocationAId]
GO
ALTER TABLE [dbo].[Graph]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Graph_dbo.Location_LocationBId] FOREIGN KEY([LocationBId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Graph] CHECK CONSTRAINT [FK_dbo.Graph_dbo.Location_LocationBId]
GO
ALTER TABLE [dbo].[Information]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Information_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Information] CHECK CONSTRAINT [FK_dbo.Information_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Instrument]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Instrument_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Instrument] CHECK CONSTRAINT [FK_dbo.Instrument_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Line]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Line_dbo.Location_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Line] CHECK CONSTRAINT [FK_dbo.Line_dbo.Location_DestinationId]
GO
ALTER TABLE [dbo].[Line]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Line_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Line] CHECK CONSTRAINT [FK_dbo.Line_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Line]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Line_dbo.Location_OriginId] FOREIGN KEY([OriginId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Line] CHECK CONSTRAINT [FK_dbo.Line_dbo.Location_OriginId]
GO
ALTER TABLE [dbo].[Load]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Load_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Load] CHECK CONSTRAINT [FK_dbo.Load_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Location_dbo.Location_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_dbo.Location_dbo.Location_ParentId]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Location_dbo.Type_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Type] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_dbo.Location_dbo.Type_TypeId]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Log_dbo.Application_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_dbo.Log_dbo.Application_ApplicationId]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Log_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_dbo.Log_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[Logbook]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Logbook_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Logbook] CHECK CONSTRAINT [FK_dbo.Logbook_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[Origin]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Origin_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Origin] CHECK CONSTRAINT [FK_dbo.Origin_dbo.Location_Id]
GO
ALTER TABLE [dbo].[OxD]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OxD_dbo.Location_DestinationId] FOREIGN KEY([DestinationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[OxD] CHECK CONSTRAINT [FK_dbo.OxD_dbo.Location_DestinationId]
GO
ALTER TABLE [dbo].[OxD]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OxD_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[OxD] CHECK CONSTRAINT [FK_dbo.OxD_dbo.Location_Id]
GO
ALTER TABLE [dbo].[OxD]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OxD_dbo.Location_OriginId] FOREIGN KEY([OriginId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[OxD] CHECK CONSTRAINT [FK_dbo.OxD_dbo.Location_OriginId]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Permission_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK_dbo.Permission_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Plc]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Plc_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Plc] CHECK CONSTRAINT [FK_dbo.Plc_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Position]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Position_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [FK_dbo.Position_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rConveyorCapacity]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorCapacity_dbo.Conveyor_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Conveyor] ([Id])
GO
ALTER TABLE [dbo].[rConveyorCapacity] CHECK CONSTRAINT [FK_dbo.rConveyorCapacity_dbo.Conveyor_Id]
GO
ALTER TABLE [dbo].[rConveyorConsumption]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorConsumption_dbo.Conveyor_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Conveyor] ([Id])
GO
ALTER TABLE [dbo].[rConveyorConsumption] CHECK CONSTRAINT [FK_dbo.rConveyorConsumption_dbo.Conveyor_Id]
GO
ALTER TABLE [dbo].[rConveyorInputPosition]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorInputPosition_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rConveyorInputPosition] CHECK CONSTRAINT [FK_dbo.rConveyorInputPosition_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rConveyorLenght]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorLenght_dbo.Conveyor_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Conveyor] ([Id])
GO
ALTER TABLE [dbo].[rConveyorLenght] CHECK CONSTRAINT [FK_dbo.rConveyorLenght_dbo.Conveyor_Id]
GO
ALTER TABLE [dbo].[rConveyorOutputPosition]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorOutputPosition_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rConveyorOutputPosition] CHECK CONSTRAINT [FK_dbo.rConveyorOutputPosition_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rConveyorScale]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorScale_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rConveyorScale] CHECK CONSTRAINT [FK_dbo.rConveyorScale_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rConveyorSpeed]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rConveyorSpeed_dbo.Conveyor_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Conveyor] ([Id])
GO
ALTER TABLE [dbo].[rConveyorSpeed] CHECK CONSTRAINT [FK_dbo.rConveyorSpeed_dbo.Conveyor_Id]
GO
ALTER TABLE [dbo].[Reference]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reference_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Reference] CHECK CONSTRAINT [FK_dbo.Reference_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Reference]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reference_dbo.Location_LocReferenceId] FOREIGN KEY([LocReferenceId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Reference] CHECK CONSTRAINT [FK_dbo.Reference_dbo.Location_LocReferenceId]
GO
ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Resource_dbo.Type_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Type] ([Id])
GO
ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_dbo.Resource_dbo.Type_TypeId]
GO
ALTER TABLE [dbo].[Reversal]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reversal_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Reversal] CHECK CONSTRAINT [FK_dbo.Reversal_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rInstrumentMeasure]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rInstrumentMeasure_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rInstrumentMeasure] CHECK CONSTRAINT [FK_dbo.rInstrumentMeasure_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rInstrumentMeasure]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rInstrumentMeasure_dbo.Tag_TagId] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[rInstrumentMeasure] CHECK CONSTRAINT [FK_dbo.rInstrumentMeasure_dbo.Tag_TagId]
GO
ALTER TABLE [dbo].[rLocationDataHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLocationDataHistory_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rLocationDataHistory] CHECK CONSTRAINT [FK_dbo.rLocationDataHistory_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rLocationEnable]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLocationEnable_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rLocationEnable] CHECK CONSTRAINT [FK_dbo.rLocationEnable_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rLocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLocationHistory_dbo.History_HistoryId] FOREIGN KEY([HistoryId])
REFERENCES [dbo].[History] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rLocationHistory] CHECK CONSTRAINT [FK_dbo.rLocationHistory_dbo.History_HistoryId]
GO
ALTER TABLE [dbo].[rLocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLocationHistory_dbo.Location_Location_Id] FOREIGN KEY([Location_Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rLocationHistory] CHECK CONSTRAINT [FK_dbo.rLocationHistory_dbo.Location_Location_Id]
GO
ALTER TABLE [dbo].[rLocationValue]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLocationValue_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rLocationValue] CHECK CONSTRAINT [FK_dbo.rLocationValue_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rLogbookEPEE]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLogbookEPEE_dbo.Logbook_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Logbook] ([Id])
GO
ALTER TABLE [dbo].[rLogbookEPEE] CHECK CONSTRAINT [FK_dbo.rLogbookEPEE_dbo.Logbook_Id]
GO
ALTER TABLE [dbo].[rLogbookHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLogbookHistory_dbo.History_HistoryId] FOREIGN KEY([HistoryId])
REFERENCES [dbo].[History] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rLogbookHistory] CHECK CONSTRAINT [FK_dbo.rLogbookHistory_dbo.History_HistoryId]
GO
ALTER TABLE [dbo].[rLogbookHistory]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLogbookHistory_dbo.Logbook_LogbookId] FOREIGN KEY([LogbookId])
REFERENCES [dbo].[Logbook] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rLogbookHistory] CHECK CONSTRAINT [FK_dbo.rLogbookHistory_dbo.Logbook_LogbookId]
GO
ALTER TABLE [dbo].[rLogbookVV]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rLogbookVV_dbo.Logbook_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Logbook] ([Id])
GO
ALTER TABLE [dbo].[rLogbookVV] CHECK CONSTRAINT [FK_dbo.rLogbookVV_dbo.Logbook_Id]
GO
ALTER TABLE [dbo].[Route]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Route_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Route] CHECK CONSTRAINT [FK_dbo.Route_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rProductionBoarding]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Instrument_InstrumentId] FOREIGN KEY([InstrumentId])
REFERENCES [dbo].[Instrument] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rProductionBoarding] CHECK CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Instrument_InstrumentId]
GO
ALTER TABLE [dbo].[rProductionBoarding]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Location_BerthId] FOREIGN KEY([BerthId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rProductionBoarding] CHECK CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Location_BerthId]
GO
ALTER TABLE [dbo].[rProductionBoarding]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Production_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Production] ([Id])
GO
ALTER TABLE [dbo].[rProductionBoarding] CHECK CONSTRAINT [FK_dbo.rProductionBoarding_dbo.Production_Id]
GO
ALTER TABLE [dbo].[rProductionOrigin]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionOrigin_dbo.Production_ProductionId] FOREIGN KEY([ProductionId])
REFERENCES [dbo].[Production] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rProductionOrigin] CHECK CONSTRAINT [FK_dbo.rProductionOrigin_dbo.Production_ProductionId]
GO
ALTER TABLE [dbo].[rProductionRoute]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionRoute_dbo.Instrument_InstrumentId] FOREIGN KEY([InstrumentId])
REFERENCES [dbo].[Instrument] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rProductionRoute] CHECK CONSTRAINT [FK_dbo.rProductionRoute_dbo.Instrument_InstrumentId]
GO
ALTER TABLE [dbo].[rProductionRoute]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionRoute_dbo.Location_RouteId] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rProductionRoute] CHECK CONSTRAINT [FK_dbo.rProductionRoute_dbo.Location_RouteId]
GO
ALTER TABLE [dbo].[rProductionRoute]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionRoute_dbo.Production_ProductionId] FOREIGN KEY([ProductionId])
REFERENCES [dbo].[Production] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rProductionRoute] CHECK CONSTRAINT [FK_dbo.rProductionRoute_dbo.Production_ProductionId]
GO
ALTER TABLE [dbo].[rProductionStock]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionStock_dbo.Instrument_InstrumentId] FOREIGN KEY([InstrumentId])
REFERENCES [dbo].[Instrument] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rProductionStock] CHECK CONSTRAINT [FK_dbo.rProductionStock_dbo.Instrument_InstrumentId]
GO
ALTER TABLE [dbo].[rProductionStock]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rProductionStock_dbo.Production_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Production] ([Id])
GO
ALTER TABLE [dbo].[rProductionStock] CHECK CONSTRAINT [FK_dbo.rProductionStock_dbo.Production_Id]
GO
ALTER TABLE [dbo].[rRouteActive]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteActive_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteActive] CHECK CONSTRAINT [FK_dbo.rRouteActive_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rRouteGraphGpv]  WITH CHECK ADD  CONSTRAINT [FK_dbo.RouteGraphGpv_dbo.RouteGraph_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[RouteGraph] ([Id])
GO
ALTER TABLE [dbo].[rRouteGraphGpv] CHECK CONSTRAINT [FK_dbo.RouteGraphGpv_dbo.RouteGraph_Id]
GO
ALTER TABLE [dbo].[rRouteGraphLocation]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteGraphLocation_dbo.RouteGraph_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[RouteGraph] ([Id])
GO
ALTER TABLE [dbo].[rRouteGraphLocation] CHECK CONSTRAINT [FK_dbo.rRouteGraphLocation_dbo.RouteGraph_Id]
GO
ALTER TABLE [dbo].[rRouteGraphSequence]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteGraphSequence_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rRouteGraphSequence] CHECK CONSTRAINT [FK_dbo.rRouteGraphSequence_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rRouteGraphSequence]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteGraphSequence_dbo.RouteGraph_RouteGraphId] FOREIGN KEY([RouteGraphId])
REFERENCES [dbo].[RouteGraph] ([Id])
GO
ALTER TABLE [dbo].[rRouteGraphSequence] CHECK CONSTRAINT [FK_dbo.rRouteGraphSequence_dbo.RouteGraph_RouteGraphId]
GO
ALTER TABLE [dbo].[rRouteOxD]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteOxD_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteOxD] CHECK CONSTRAINT [FK_dbo.rRouteOxD_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rRouteOxD]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteOxD_dbo.OxD_OxDId] FOREIGN KEY([OxDId])
REFERENCES [dbo].[OxD] ([Id])
GO
ALTER TABLE [dbo].[rRouteOxD] CHECK CONSTRAINT [FK_dbo.rRouteOxD_dbo.OxD_OxDId]
GO
ALTER TABLE [dbo].[rRouteQueue]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteQueue_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteQueue] CHECK CONSTRAINT [FK_dbo.rRouteQueue_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rRouteReplace]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteReplace_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteReplace] CHECK CONSTRAINT [FK_dbo.rRouteReplace_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rRouteSequence]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteSequence_dbo.Location_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteSequence] CHECK CONSTRAINT [FK_dbo.rRouteSequence_dbo.Location_LocationId]
GO
ALTER TABLE [dbo].[rRouteSequence]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteSequence_dbo.Location_RouteId] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Location] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rRouteSequence] CHECK CONSTRAINT [FK_dbo.rRouteSequence_dbo.Location_RouteId]
GO
ALTER TABLE [dbo].[rRouteTravel]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRouteTravel_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[rRouteTravel] CHECK CONSTRAINT [FK_dbo.rRouteTravel_dbo.Location_Id]
GO
ALTER TABLE [dbo].[rRuleNavbar]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rRuleNavbar_dbo.Rule_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Rule] ([Id])
GO
ALTER TABLE [dbo].[rRuleNavbar] CHECK CONSTRAINT [FK_dbo.rRuleNavbar_dbo.Rule_Id]
GO
ALTER TABLE [dbo].[rTagConveyorClean]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagConveyorClean_dbo.Conveyor_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Conveyor] ([Id])
GO
ALTER TABLE [dbo].[rTagConveyorClean] CHECK CONSTRAINT [FK_dbo.rTagConveyorClean_dbo.Conveyor_Id]
GO
ALTER TABLE [dbo].[rTagConveyorClean]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagConveyorClean_dbo.Tag_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[rTagConveyorClean] CHECK CONSTRAINT [FK_dbo.rTagConveyorClean_dbo.Tag_Id]
GO
ALTER TABLE [dbo].[rTagGroup]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagGroup_dbo.rTagGroup_ParentId] FOREIGN KEY([ParentId])
REFERENCES [dbo].[rTagGroup] ([Id])
GO
ALTER TABLE [dbo].[rTagGroup] CHECK CONSTRAINT [FK_dbo.rTagGroup_dbo.rTagGroup_ParentId]
GO
ALTER TABLE [dbo].[rTagGroup]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagGroup_dbo.Tag_TagId] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[rTagGroup] CHECK CONSTRAINT [FK_dbo.rTagGroup_dbo.Tag_TagId]
GO
ALTER TABLE [dbo].[rTagInformation]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagInformation_dbo.Tag_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[rTagInformation] CHECK CONSTRAINT [FK_dbo.rTagInformation_dbo.Tag_Id]
GO
ALTER TABLE [dbo].[rTagWrite]  WITH CHECK ADD  CONSTRAINT [FK_dbo.rTagWrite_dbo.Tag_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Tag] ([Id])
GO
ALTER TABLE [dbo].[rTagWrite] CHECK CONSTRAINT [FK_dbo.rTagWrite_dbo.Tag_Id]
GO
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Rule_dbo.Application_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_dbo.Rule_dbo.Application_ApplicationId]
GO
ALTER TABLE [dbo].[Step]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Step_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Step] CHECK CONSTRAINT [FK_dbo.Step_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Stock_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_dbo.Stock_dbo.Location_Id]
GO
ALTER TABLE [dbo].[Tag]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tag_dbo.Plc_PlcId] FOREIGN KEY([PlcId])
REFERENCES [dbo].[Plc] ([Id])
GO
ALTER TABLE [dbo].[Tag] CHECK CONSTRAINT [FK_dbo.Tag_dbo.Plc_PlcId]
GO
ALTER TABLE [dbo].[Tripper]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Tripper_dbo.Location_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Tripper] CHECK CONSTRAINT [FK_dbo.Tripper_dbo.Location_Id]
GO
/****** Object:  StoredProcedure [dbo].[sp_Production_All_Clean]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cleber Ferreira e Davi Sartori
-- Create date: 12/07/2024
-- Description: Escopo Descarga e Embarque | Elimina todos as produções fechadas sem carga no destino
-- =============================================
CREATE PROCEDURE [dbo].[sp_Production_All_Clean]
as
BEGIN
	   
	   -------- Inicio: Tabela temporária com registros desnecessários para eliminação
		create table #Delete (Id uniqueidentifier, ProductionId uniqueidentifier)
		insert into #Delete (Id, ProductionId) -- insert na tabela temporária
		SELECT rProductionRoute.[Id] -- Lista os registros finalizados a mais de 2 horas que o destino não tem carga
			  ,[ProductionId]
		  FROM [dbo].[rProductionRoute]
		  inner join rProductionStock on rProductionRoute.ProductionId = rProductionStock.Id
		  inner join Production on Production.Id = rProductionStock.Id
		  where 
		  Production.Final = 1 and rProductionStock.Load = 0 and 
		  DATEDIFF(HOUR,Production.dhf, getdate()) > = 2
		-------- Fim
		
		-------- Inicio: Deleta registros baseado na tabela temporária
		-- Deleta ProductionRoute
		delete from rProductionRoute
		from rProductionRoute
		inner join #Delete on #Delete.Id = rProductionRoute.Id
		-- Deleta ProductionStock
		delete from rProductionStock
		from rProductionStock
		inner join (select distinct ProductionId from #Delete) as #Delete on #Delete.ProductionId = rProductionStock.Id
		-- Deleta a Production
		delete from Production
		from Production
		inner join (select distinct ProductionId from #Delete) as #Delete on #Delete.ProductionId = Production.Id
		-------- Fim



		--Elimina a tabela temporária
		drop table #Delete

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Production_All_Shift_Closing]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cleber Ferreira
-- Create date: 12/11/2023
-- Description:	Lógica de fechamento de turno de 6 horas
-- =============================================
CREATE PROCEDURE [dbo].[sp_Production_All_Shift_Closing]
AS
BEGIN
-- If que verifica se é o primeiro minuto do turno de 6 horas
	if 
	(( 
	DATEPART(hour,getdate()) = 0 and DATEPART(MINUTE,getdate()) = 0 or
	DATEPART(hour,getdate()) = 6 and DATEPART(MINUTE,getdate()) = 0 or
	DATEPART(hour,getdate()) = 12 and DATEPART(MINUTE,getdate()) = 0 or
	DATEPART(hour,getdate()) = 18 and DATEPART(MINUTE,getdate()) = 0
	) and
	-- Aqui verifica se já existiu um tratamento neste minuto, caso negativo permite rodar o codigo
	(select count(*) from rProductionRoute where 
	DATEPART(day,dhi) = DATEPART(day,getdate()) and
	DATEPART(MONTH,dhi) = DATEPART(MONTH,getdate()) and
	DATEPART(YEAR,dhi) = DATEPART(YEAR,getdate()) and
	DATEPART(HOUR,dhi) = DATEPART(hour,getdate()) and
	DATEPART(MINUTE,dhi) = DATEPART(MINUTE,getdate())) = 0 
	)
	begin
		-- Finaliza as produções atuais
		UPDATE [dbo].[Production]  SET [Final] = 1, Active = 0, dhf = getdate() WHERE Final = 0
		UPDATE [dbo].[rProductionRoute]  SET [Final] = 1, Active = 0, dhf = getdate(), Cleaning = 1, Gate = 'Fechamento Turno'  WHERE Final = 0
		-- Recria as produções atuais
		exec sp_Production_Discharge_Ins
		exec sp_Production_Discharge_Upd
		-- Atualiza o numero de vagões inicial
		update[Route].[dbo].[rProductionRoute]
		set InitialNWagon = meas.Value
		FROM [Route].[dbo].[rProductionRoute]
		inner join rRouteOxD on rRouteOxD.Id = rProductionRoute.RouteId
		inner join OxD on OxD.Id = rRouteOxD.OxDId
		inner join [vw_Tag_Overturned_Wagons_Instrument] as vw on vw.LocationId = oxd.OriginId
		inner join rInstrumentMeasure as meas on meas.Id = vw.InstrumentId
		where
		InitialNWagon = -1

	end



END
GO
/****** Object:  StoredProcedure [dbo].[sp_Production_Discharge_Ins]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira e Davi Sartori
-- Create date: 20/07/2023
-- Description:	Cria um novo registro de produção para consumo do sistema GPV Em rotas com origens de virador de vagões
-- =============================================
CREATE PROCEDURE [dbo].[sp_Production_Discharge_Ins]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-----------Inicio: Criar tabelas temporárias para eliminar multiplas consultas na mesma fonte do banco de dados
	----------- e aumentar a performance da procedure

	--## Cria tabela temporária e insere rotas ativas cuja a origem são viradores de vagões
	create table #rRouteActive (Id int) 
	insert into #rRouteActive(id)
	SELECT rRouteActive.Id
				FROM [Route].[dbo].[rRouteActive]
				inner join rRouteOxD on rRouteOxD.Id = rRouteActive.Id
				inner join OxD on OxD.Id = rRouteOxD.OxDId
				where
				OxD.OriginId >= 209 and OxD.OriginId <= 213 -- Ids dos viradores de vagões

	--## Cria tabela temporaria e insere dados onde cada registro é um rProductionRoute a ser cadastrado
	create table #tbTempProdRoute (id uniqueidentifier, RouteId int, dhi datetime)

	-- Levanta rotas ativas e nenhum rproductionroute associado - Devo cadastrar rproductionroute 
	--e informar na tabela #tbTempProdRoute 
	insert into #tbTempProdRoute (id, RouteId, dhi)
	select NEWID(), Route.Id, GETDATE()
	FROM #rRouteActive as Route
	inner join Location on Location.Id = Route.Id
	left outer join rProductionRoute on rProductionRoute.RouteId = Route.Id and rProductionRoute.Final = 0
	where
	rProductionRoute.Id is null 


	--## Cria tabela temporaria e insere dados onde cada registro é um Production já existente 
	-- para relacionar ao rProductionRoute
	create table #tbTempProdExistente (id uniqueidentifier, DestinationId int, dhi datetime)

	-- Descobrir os productions que já estão cadastrados e informar na tabela #tbTempProdExistente
	insert into #tbTempProdExistente (id, DestinationId)
	SELECT Production.Id, DestinationId
	FROM [Route].[dbo].[Production]
	inner join rProductionRoute on rProductionRoute.ProductionId = Production.Id
	inner join rRouteOxD on rRouteOxD.Id = rProductionRoute.RouteId
	inner join OxD on OxD.Id = rRouteOxD.OxDId
	where
	Production.Final = 0 and rProductionRoute.Final = 0
	group by Production.Id, DestinationId


	--## Cria tabela temporaria e insere dados onde cada registro é um Production a ser cadastrado
	create table #tbTempProdNova (id uniqueidentifier, DestinationId int, dhi datetime)

	-- Verifica quais os rPRoductionRoute que desejo cadastra e não possuem Productions ID
	-- Guardo essa informação na tabela #tbTempProdNova

	insert into #tbTempProdNova (id,DestinationId , dhi)
	select NEWID(), OxD.DestinationId, GETDATE() 
	from #tbTempProdRoute
	inner join rRouteOxD as roxd on roxd.Id = #tbTempProdRoute.RouteId
	inner join OxD on OxD.Id = roxd.OxDId
	left outer join #tbTempProdExistente on #tbTempProdExistente.DestinationId = OxD.DestinationId
	where
	#tbTempProdExistente.DestinationId is null
	group by OxD.DestinationId

	-----------Fim

	-----------Inicio: AA Realiza o cadastro dos rProductionRoute que já possuem Production final = 0
	INSERT INTO [dbo].[rProductionRoute](Id,ProductionId,RouteId,dhi,dhf,Load,Final, Active, NWagon,InitialLoad,InitialNWagon, Dismember)
	select  #tbTempProdRoute.id, t2.id, #tbTempProdRoute.RouteId, #tbTempProdRoute.dhi, null, 0, 0,0,0,0,0-1,0
	from #tbTempProdRoute
	inner join rRouteOxD as roxd on roxd.Id = #tbTempProdRoute.RouteId
	inner join OxD on OxD.Id = roxd.OxDId
	inner join (
				select tb.DestinationId, max(id) as id 
				from #tbTempProdExistente as tb
				group by tb.DestinationId
				) as t2 on t2.DestinationId = OxD.DestinationId

	-----------Fim

	-----------Inicio:AB Cadastrar os Production novos das rotas que não possuem

	insert into Production (Id,dhi,dhf,Final,Active)
	select id uniqueidentifier,dhi datetime, null, 0, 0 from #tbTempProdNova
	
	-----------Fim

	-----------Inicio:AC Cadastrar os rProductionStock novos das rotas que não possuem
	insert into rProductionStock (Id, GoalI, GoalF, Load)
	select id uniqueidentifier,0, 0, 0 from #tbTempProdNova

	-----------Fim

	-----------Inicio:AD Cadastrar rProductionRoute dos Production criados

	INSERT INTO [dbo].[rProductionRoute](Id,ProductionId,RouteId,dhi,dhf,Load,Final, Active, NWagon,InitialLoad,InitialNWagon, Dismember)
	select  #tbTempProdRoute.id, #tbTempProdNova.id, #tbTempProdRoute.RouteId, #tbTempProdRoute.dhi, null, 0, 0,0,0,0,0-1,0
	from #tbTempProdRoute
	inner join rRouteOxD as roxd on roxd.Id = #tbTempProdRoute.RouteId
	inner join OxD on OxD.Id = roxd.OxDId
	inner join #tbTempProdNova on #tbTempProdNova.DestinationId = OxD.DestinationId
	-----------Fim

	-----------Inicio: Destruindo tabelas temporarias
	Drop Table #tbTempProdExistente
	Drop Table #tbTempProdNova
	Drop Table #tbTempProdRoute
	drop table #rRouteActive

	-----------Fim


--Rotas ativas sem production route associaddo
--#tbTempProdRoute

--Os prodoctions com seus destinos ativos no momento
--#tbTempProdExistente

--Destinos sem prodoctions ativos no momento
--#tbTempProdNova

--AA - Cadastra rotas ativas sem production no rproductionroute em productions existentes (de mesmo destino)

--AB - Cadastrar os Production novos das rotas que não possuem Production

--AC - Cadastrar os rProductionStock novos das rotas que não possuem

--AD - Cadastrar rProductionRoute dos novos Production criados


END
GO
/****** Object:  StoredProcedure [dbo].[sp_Production_Discharge_Upd]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cleber Ferreira e Davi Sartori
-- Create date: 27/07/2023
-- Description:	Storage Procedure que atualizar os valores das Productions em operação
-- =============================================
CREATE PROCEDURE [dbo].[sp_Production_Discharge_Upd]
AS
BEGIN

	-----------Inicio: Criar tabelas temporárias para eliminar multiplas consultas na mesma fonte do banco de dados
	----------- e aumentar a performance da procedure

	--## Cria tabela temporária e insere rotas ativas cuja a origem são viradores de vagões		
		create table #rRouteActive (Id int) -- Rotas ativas. Só virador
		insert into #rRouteActive(id)
		SELECT rRouteActive.Id
				FROM [Route].[dbo].[rRouteActive]
				inner join rRouteOxD on rRouteOxD.Id = rRouteActive.Id
				inner join OxD on OxD.Id = rRouteOxD.OxDId
				where
				OxD.OriginId >= 209 and OxD.OriginId <= 213
	
	--## Cria tabela temporaria com dados da balança da origem e rota
		create table #tb_Origin (RouteId int, value float, InstrumentId int)

		insert into #tb_Origin (RouteId, value, InstrumentId)
		select RouteId, Value, InstrumentId From vw_Route_Scale_Origin

	-----------Fim

	-----------Inicio: Atualiza o active de acordo com a primeira origem que produzir (totalização maior que 20)
		UPDATE [dbo].[Production]
		SET [Active] = 1
		FROM [Route].[dbo].[Production]
		inner join rProductionRoute as pRoute on pRoute.ProductionId = Production.Id 
		inner join #tb_Origin as Scale on Scale.RouteId = pRoute.RouteId
		where
		Production.Final = 0 and Production.Active = 0 and case when (Value - InitialLoad) > 20.0 then 1 else 0 end = 1 and InitialLoad > 0
	-----------Fim

	-----------Inicio:  Atualiza o dhi (data e hora inicial) enquanto não estiver ativo, indicando o momento que ficou ativo)
		UPDATE [dbo].[Production]
		SET dhi = getdate()
		FROM [Route].[dbo].[Production]
		inner join rProductionRoute as pRoute on pRoute.ProductionId = Production.Id 
		where
		Production.Final = 0 and Production.Active = 0
	-----------Fim

	-----------Inicio:  Query que atualiza a data final enquanto final é igual 0 e active = 1
		UPDATE [dbo].[Production]
		SET dhf = GETDATE()
		from Production
		where
		Final = 0 and Active = 1
	-----------Fim

	-----------Inicio:  Query finaliza a produção caso não tenha mais rota ativa
		UPDATE [dbo].[Production]
	    SET [Final] = 1
		FROM Production as p 
		inner join [Route].[dbo].[rProductionRoute] as rp  on p.Id = rp.ProductionId
		inner join
		(
			select ProductionId, SUM(active.Id) as SumAct, SUM(rp.RouteId) as SumProd
			FROM Production as p 
			inner join [Route].[dbo].[rProductionRoute] as rp  on p.Id = rp.ProductionId
			left outer join #rRouteActive as active on active.Id = rp.RouteId
			where
			P.Final = 0
			group by ProductionId
		) as T2 on t2.ProductionId = p.Id
		where
		SumAct is null
	-----------Fim

	-----------Inicio:  Se todos os rProductionRoute estiverem finalizados o Production é finalizado
		update Production
		set final = 1
		from Production
		inner join
		(select ProductionId, SUM(cast(Final as int)) as SumFinal, COUNT(*) as Qtd from rProductionRoute
		group by ProductionId) as T1 on T1.ProductionId = Production.Id
		where
		T1.Qtd = T1.SumFinal and Production.Final = 0
	-----------Fim

	-----------Inicio: Destruindo tabelas temporarias
		drop table #rRouteActive
		drop table #tb_Origin
	-----------Fim
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Production_Exec_Rules]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cleber Ferreira / Davi Sartori
-- Create date: 28/08/23
-- Description:	Gerencia os registros de produção para geração de fechamento de relatório para o sistema GPV
-- =============================================
CREATE PROCEDURE [dbo].[sp_Production_Exec_Rules]
AS
BEGIN

	BEGIN TRY  

		SET NOCOUNT ON;

		------------- Inicio - Temporário: Lê rotas aativas do lab (TU-HF-DTO-VS19) e copia aqui no PS-DP-DTO-VS01
		DELETE FROM [dbo].[rRouteActive]
		FROM [Route].[dbo].[rRouteActive]
		left outer join [TU-HF-DTO-VS19].[Route].[dbo].[rRouteActive] as Remoto on rRouteActive.Id = Remoto.Id
		where
		Remoto.Id is null

		INSERT INTO Route.[dbo].[rRouteActive]
			([Id]
			,[dh])
		SELECT Remoto.[Id],Remoto.[dh]
		FROM [TU-HF-DTO-VS19].[Route].[dbo].[rRouteActive] as Remoto 
		left outer join [Route].[dbo].[rRouteActive] on rRouteActive.Id = Remoto.Id 
		where
		rRouteActive.Id is null

		-------------- Fim

		-------------- Inicio: Executas as procedures de registro de produção
		declare @dh datetime = getdate() -- Guarda momento para medição de tempo de computação
		exec sp_Production_All_Shift_Closing -- executa procedure de fechamento de turno 0h, 6h, 12h ,18h
		exec sp_ProductionRoute_Discharge_Upd -- executa procedure atualiza registro de dados da rota (Origem)
		exec sp_ProductionStock_Discharge_Upd -- executa procedure atualiza registro dados do estoque (Destino)
		exec sp_Production_Discharge_Ins -- executa procedure cadastra nova produção
		exec sp_Production_Discharge_Upd -- executa procedure atualiza informações de uma produção
		exec sp_Production_All_Clean -- executa procedure limpa registros desnecessários

		-------------- Fim

		-------------- Inicio: Registra histórico do tempo de computação 
		INSERT INTO [dbo].[rLocationDataHistory]
           ([Id]
           ,[LocationId]
           ,[dh]
           ,[OperationTime])
		 VALUES
			   (NEWID()
			   ,1
			   ,GETDATE()
			   ,DATEDIFF(MILLISECOND, @dh, GETDATE())) -- diferença entre agora e antes de rodar as procedures
		
		if (DATEDIFF(MILLISECOND, @dh, GETDATE()) > 5000)
		begin
			INSERT INTO [dbo].[Log]
				([Id] ,[ApplicationId] ,[User] ,[dh] ,[Message] ,[LocationId])
			VALUES
				(NEWID() ,2 ,'Sistema' ,getdate()
				,'Tempo alto de processamento de ' + cast( DATEDIFF(MILLISECOND, @dh, GETDATE()) as varchar (100))
				,13588) 
		 end
		-- Elimina o histórico de antes de 1 hora atrás
		select @dh = min(dh) from [dbo].[rLocationDataHistory] --
				
		DELETE FROM [dbo].[rLocationDataHistory]
		WHERE DATEDIFF(hour,(select min(dh) from [dbo].[rLocationDataHistory]),getdate()) >1 and LocationId = 1

		
		-------------- Fim

	END TRY  
	BEGIN CATCH  
		--- Em caso de erro os detalhes são registrados no log
		select 'There is a problem!'
		INSERT INTO [dbo].[Log]
				   ([Id]
				   ,[ApplicationId]
				   ,[User]
				   ,[dh]
				   ,[Message])
				   select NEWID(), 2, 'Windows Service', GETDATE(), 'Error Number: ' + ERROR_NUMBER() + ' - message: ' + ERROR_MESSAGE() 
	END CATCH  

END
GO
/****** Object:  StoredProcedure [dbo].[sp_ProductionRoute_Discharge_Upd]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira / Davi Sartori
-- Create date: 21/07/2023
-- Description:	Atualiza a totalização das origens
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProductionRoute_Discharge_Upd]
AS
BEGIN
	declare @TimeK float = 3.5
	-----------Inicio: Criar tabelas temporárias para eliminar multiplas consultas na mesma fonte do banco de dados
	----------- e aumentar a performance da procedure
	--## Cria tabela temporária e insere rotas ativas cuja a origem são viradores de vagões e seus tempos de viagem
		create table #rRouteActive (Id int) -- Rotas ativas. Só virador
		insert into #rRouteActive(id)
		SELECT rRouteActive.Id
				FROM [Route].[dbo].[rRouteActive]
				inner join rRouteOxD on rRouteOxD.Id = rRouteActive.Id
				inner join OxD on OxD.Id = rRouteOxD.OxDId
				where
				OxD.OriginId >= 209 and OxD.OriginId <= 213
		-- Criação das tabelas temporárias para performance

	--## Cria tabela temporária e para ler Vagões virados
		create table #Overturned_Wagons_Inst ([Location] varchar (100) ,[LocationId] int
		,[Tag] varchar (100),[TagId] int ,[Plc] varchar (100) ,[PlcId] int ,[InstrumentId] int
		,[Value] int)

		insert into #Overturned_Wagons_Inst ([Location] ,[LocationId] ,[Tag]
		,[TagId] ,[Plc] ,[PlcId] ,[InstrumentId] ,[Value])
		SELECT [Location] ,[LocationId] ,[Tag]
		,[TagId] ,[Plc] ,[PlcId] ,[InstrumentId] ,cast(Value as int)
		FROM [Route].[dbo].[vw_Tag_Overturned_Wagons_Instrument]

	--## Cria tabela temporária e para ler Balanças de origem
		create table #OriginScale (RouteId int, value float, instId int)

		insert into #OriginScale (RouteId, value, instId)
		select RouteId, Value, InstrumentId From vw_Route_Scale_Origin

	--## Cria tabela temporária criando tabela OxD vs Rotas ativas
		create table #rRouteOxD (RouteId int, OxDId int, DestinationId int, OriginId int)
		insert into #rRouteOxD (RouteId, OxDId, DestinationId, OriginId)
		select #rRouteActive.Id, OxD.Id, DestinationId, OriginId from #rRouteActive
		inner join rRouteOxD on rRouteOxD.Id = #rRouteActive.Id
		inner join OxD on OxD.Id = rRouteOxD.OxDId


	--## Cria tabela temporária lendo o Tempo das rotas de Descarga
		create table #vw_Route_Origin_Time_To_Scale (Id int, Time float)
		insert into #vw_Route_Origin_Time_To_Scale (Id, Time)
		select Id, Time from vw_Route_Origin_Time_To_Scale

		
	-----------Fim

	-----------Inicio: Atualiza o número inicial de vagões virados e némero de desmembramentos
		update [Route].[dbo].[rProductionRoute]
		set InitialNWagon = vw_nwag.Value, Dismember = meas_dism.Value
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join #Overturned_Wagons_Inst as vw_nwag on vw_nwag.LocationId = #rRouteOxD.OriginId
		inner join vw_Tag_Dismember_Instrument as vw_dism on vw_dism.LocationId = #rRouteOxD.OriginId
		inner join rInstrumentMeasure as meas_dism on meas_dism.Id = vw_dism.InstrumentId
		where
		InitialNWagon is null or InitialNWagon = -1

	-----------Fim

	-----------Inicio: Verifica se o valor do numero de vagões dará negativo, se isso ocorre escreva o initialNWagon = 0
		update[Route].[dbo].[rProductionRoute]
		set InitialNWagon = 0
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join #Overturned_Wagons_Inst as vw on vw.LocationId = #rRouteOxD.OriginId
		where
		Final = 0 and NWagon < Value and (Value - isnull(InitialNWagon,0)) < 0

	-----------Fim

	-----------Inicio: Atualiza a informação de numeros de vagões virados
		update[Route].[dbo].[rProductionRoute]
		set NWagon = Value - case when InitialNWagon > 0 then InitialNWagon else 0 end
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join #Overturned_Wagons_Inst as vw on vw.LocationId = #rRouteOxD.OriginId
		where
		Final = 0 and NWagon < Value

	-----------Fim

	-- TRATAMENTO DE BALANÇAS. 
	-----------Inicio: Armazena o valor inicial do totalizador 
		UPDATE [dbo].[rProductionRoute]
		SET InitialLoad = bal.Value, InstrumentId = Bal.instId
		from rProductionRoute 
		inner join #OriginScale as bal on rProductionRoute.RouteId = bal.RouteId
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 0 and NWagon = 0 
		and (InitialLoad is null or InitialLoad = 0)
	-----------Fim

	-----------Inicio: Seta o Active = 1 da rota sinalizando inicio da produção
		UPDATE [dbo].[rProductionRoute]
		SET Active = 1, dhi = getdate()
		FROM [Route].[dbo].rProductionRoute
		inner join #OriginScale on #OriginScale.RouteId = rProductionRoute.RouteId
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 0 and 
		case when (#OriginScale.value - InitialLoad) > 20.0 then 1 else 0 end = 1 and InitialLoad > 0
	-----------Fim

	-----------Inicio:  Modifica o valor da produção (rProductionRoute) de acordo com a totalização da Origem
		UPDATE [dbo].[rProductionRoute]
		SET [Load] = bal.Value - InitialLoad
		from rProductionRoute 
		inner join #OriginScale as bal on rProductionRoute.RouteId = bal.RouteId
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 1 		
	-----------Fim

	-----------Inicio:  Modifica a rota em casos específicos (EP03, EP10, 5PA4, 1PA2)
		update [Route].[dbo].[rProductionRoute]
		set dhFinalRouteId = GETDATE()
		FROM [Route].[dbo].[rProductionRoute]
		where
		dhFinalRouteId is null

		UPDATE [dbo].[rProductionRoute]
		SET 
		FinalRouteId = dbo.fn_RouteAjust_I_Route(rProductionRoute.RouteId),
		dhFinalRouteId = GETDATE()
		from rProductionRoute 
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 1 	and
		(DATEDIFF(second,dhFinalRouteId,getdate()) >= 600 or dhFinalRouteId is null)
		and (FinalRouteId is null or FinalRouteId <> dbo.fn_RouteAjust_I_Route(rProductionRoute.RouteId))
	-----------Fim

	-----------Inicio:
		UPDATE [dbo].[rProductionRoute]
		SET [FinalLoad] = DestinationLoadCalc
		FROM [Route].[dbo].[rProductionRoute]
		inner join vw_Production_Rateio on rProductionRoute.Id = vw_Production_Rateio.ProductionRouteId
		where
		DestinationLoadCalc > 0
	-----------Fim

	---- CONTROLE DOS REGISTROS DE PRODUÇÃO
	-----------Inicio: 1 Finaliza a produção da rota caso a rota seja removido do sistema rotas
		update rProductionRoute 
		set Final = 1, Active = 0, Cleaning = 1, Gate = 'Removeu Rota'
		FROM [Route].[dbo].[rProductionRoute]
		inner join Production on Production.Id = rProductionRoute.ProductionId and Production.Final = 0 and rProductionRoute.Final = 0
		left outer join #rRouteActive as active on rProductionRoute.RouteId = active.Id
		where
		active.Id is null
	-----------Fim

	-----------Inicio: Finaliza a produção da rota caso a produção seja finalizada
		update rProductionRoute 
		set Final = 1, Active = 0, Cleaning = 1, Gate = 'Production Finalizada'
		from rProductionRoute
		inner join (select id, Final, ProductionId, Active, Cleaning from rProductionRoute where rProductionRoute.Final = 0) as T1 on T1.Id = rProductionRoute.Id
		left outer join (select id from Production where Production.Final = 0) 
		as Production on Production.Id = rProductionRoute.ProductionId
		where
		Production.Id is null 
	-----------Fim

	-----------Inicio:  (Origem Viradores) Finaliza a produção da rota caso o numero de vagões virados de um virador seja zerado passado 7 minuto
		update rProductionRoute 
		set Final = 1, Active = 0, Cleaning = 1, Gate = 'Vagões Zerados'
		FROM [Route].[dbo].[rProductionRoute]
		--inner join vw_Route_TimeTravel on dbo.fn_RouteAjust_I_Route(vw_Route_TimeTravel.RouteId) = rProductionRoute.RouteId
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId and rProductionRoute.Final = 0
		inner join Location on Location.Id = #rRouteOxD.OriginId
		inner join #Overturned_Wagons_Inst as vw on vw.LocationId = #rRouteOxD.OriginId
		inner join #vw_Route_Origin_Time_To_Scale as vwtime on vwTime.Id = rProductionRoute.RouteId
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 1 and 
		rProductionRoute.NWagon > vw.Value and Location.TypeId = 39 -- Só virador
	-----------Fim

	-----------Inicio: Atualiza o último valor do OxD para comparar com o novo valor no próximo pooling
		UPDATE [dbo].rInstrumentMeasure
		SET LastValue = rInstrumentMeasure.Value
		from rInstrumentMeasure
		inner join vw_Eqp_FinalProduction as vw on vw.InstrumentId = rInstrumentMeasure.Id

	-----------Fim

		---- PROCESSO DE LIMPEZA
	-----------Inicio:  Finaliza o periodo de limpeza
		UPDATE [dbo].[rProductionRoute]
				SET Cleaning = 0,  dhf = GETDATE()
		from rProductionRoute  
		inner join rProductionStock on rProductionStock.Id = rProductionRoute.ProductionId
		INNER JOIN dbo.rRouteSequence AS seq ON seq.RouteId = rProductionRoute.RouteId
		INNER JOIN dbo.Location AS tr ON tr.Id = seq.LocationId AND (tr.TypeId = 17 OR
		tr.Id = 100) INNER JOIN
		dbo.Location AS Bal ON Bal.ParentId = tr.Id AND Bal.TypeId = 26 INNER JOIN
		dbo.Location AS BalTime ON BalTime.ParentId = Bal.Id INNER JOIN
		dbo.rLocationValue AS Time ON Time.Id = BalTime.Id
		where
		Final = 1 and Cleaning = 1 and DATEDIFF(second,dhi,getdate()) >= cast(Time.ValueFloat as int)*@TimeK
		
	-----------Fim

		---- CONTROLE DO DESMEMBRAMENTO
	-----------Inicio: Finaliza a producao quando houver desmembramento
		update rProductionRoute 
		set Final = 1, Active = 0, Cleaning = 1, Gate = 'Desmembramento'
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join (SELECT Location.Id as LocationId, L2.Id as InstrumentId
		FROM [Route].[dbo].[Location]
		inner join Location as L1 on L1.ParentId = Location.Id
		inner join Location as L2 on L2.ParentId = L1.Id
		where 
		L1.TypeId = 48) as vw on vw.LocationId = #rRouteOxD.OriginId
		inner join rInstrumentMeasure as meas on meas.Id = vw.InstrumentId
		where
		rProductionRoute.Final = 0 and rProductionRoute.Active = 1 and rProductionRoute.Dismember <> meas.Value

	-----------Fim

	-----------Inicio: Atualiza o desmembramento do virador de vagoes
		update[Route].[dbo].[rProductionRoute]
		set Dismember = Value
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join vw_Tag_Dismember_Instrument as vw on vw.LocationId = #rRouteOxD.OriginId
		inner join rInstrumentMeasure as meas on meas.Id = vw.InstrumentId
		where
		Final = 0 and (Dismember < Value or Dismember is null)

	-----------Fim

	-----------Inicio: Define o numero de vagoes atual como o inicial em uma operacao de desmembramento
		update[Route].[dbo].[rProductionRoute]
		set InitialNWagon = vw2.Value
		FROM [Route].[dbo].[rProductionRoute]
		inner join #rRouteOxD on #rRouteOxD.RouteId = rProductionRoute.RouteId
		inner join (SELECT Location.Id as LocationId, L2.Id as InstrumentId
		FROM [Route].[dbo].[Location]
		inner join Location as L1 on L1.ParentId = Location.Id
		inner join Location as L2 on L2.ParentId = L1.Id
		where 
		L1.TypeId = 48) as vw1 on vw1.LocationId = #rRouteOxD.OriginId
		inner join rInstrumentMeasure as meas_dism on meas_dism.Id = vw1.InstrumentId
		inner join #Overturned_Wagons_Inst as vw2 on vw2.LocationId = #rRouteOxD.OriginId
		where
		InitialNWagon is null and meas_dism.Value > 1
	-----------Fim

	-----------Inicio:
		--- CASO A ORIGEM POSSUA UM INSTRUMENTO VIRTUAL (INEXISTENTE) E O DESTINO
		--- UM INSTRUMENTO REAL, REPLICAR O VALOR LOAD DO DESTINO NA ORIGEM (rProdutionRoute)
		UPDATE [dbo].[rProductionRoute]
		   SET [Load] = t2.Load
		FROM [Route].[dbo].[rProductionRoute] 
		inner join rInstrumentMeasure as meas_t1 on meas_t1.Id = [rProductionRoute].InstrumentId
		inner join Tag as Tag_t1 on Tag_t1.Id = meas_t1.TagId
		inner join rProductionStock as t2 on [rProductionRoute].ProductionId = t2.Id
		inner join rInstrumentMeasure as meas_t2 on meas_t2.Id = t2.InstrumentId  
		inner join Tag as Tag_t2 on Tag_t2.Id = meas_t2.TagId
		where
		[rProductionRoute].final = 0 and Tag_t1.PlcId = 7912 --- 1912 é um PLC VIRTUAL
	-----------Fim

	-----------Inicio: Destruindo tabelas temporarias

		drop table #rRouteOxD
		drop table #Overturned_Wagons_Inst
		drop table #OriginScale
		drop table #rRouteActive
		drop table #vw_Route_Origin_Time_To_Scale
	-----------Fim
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ProductionStock_Discharge_Upd]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira / Davi Sartori
-- Create date: 26/07/2023
-- Description:	Atualiza a totalização dos destinos.
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProductionStock_Discharge_Upd]
AS
BEGIN

	-----------Inicio: Criar tabelas temporárias para eliminar multiplas consultas na mesma fonte do banco de dados
	----------- e aumentar a performance da procedure
	--## Cria tabela temporária e insere rotas ativas cuja a origem são viradores de vagões e seus tempos de viagem
		create table #vw_Route_Origin_Time_To_Scale (Time float,[RouteId] int)

		insert into #vw_Route_Origin_Time_To_Scale (Time, RouteId)
		select Time, [Id] from vw_Route_Origin_Time_To_Scale
	--## Cria tabela temporária e insere rotas ativas com destinos e suas balanças
		create table #vw_Route_Scale_Destination ([DestinationId] int,[BalId]int
		,[TagId] int,[InstrumentId] int,[Value] float,[VazTot] varchar (100),[RouteId] int)

		insert into #vw_Route_Scale_Destination ([DestinationId] ,[BalId],[TagId] ,[InstrumentId] ,[Value] ,[VazTot] ,[RouteId])
		SELECT [DestinationId] ,[BalId],[TagId] ,[InstrumentId] ,cast([Value] as float) ,[VazTot] ,[RouteId]
		FROM [Route].[dbo].[vw_Route_Scale_Destination]
		
	-----------Fim

	-----------Inicio: Modifica o valor da produção (rProductionStock) de acordo com a totalização do Destino.
		UPDATE [dbo].[rProductionStock]
		SET [Load] = vw.Value - rProductionStock.InitialLoad
		FROM [Route].[dbo].[rProductionStock]
		inner join Production on Production.Id = rProductionStock.Id
		inner join [Route].[dbo].[rProductionRoute] as PR on rProductionStock.Id = PR.ProductionId
		inner join #vw_Route_Scale_Destination as vw on vw.RouteId = PR.RouteId
		where
		rProductionStock.Load <= vw.Value and rProductionStock.InitialLoad > 0 
		and Production.Final = 0 

	-----------Fim

	-----------Inicio: Se todos os viradores do destino tiver 0 virados e a carga do destino for > 0
	-----------finaliza o production
		UPDATE [dbo].[Production]
		SET final = 1
		from Production
		inner join rProductionStock  on Production.Id = rProductionStock.Id
		inner join [rProductionRoute] as PR on rProductionStock.Id = PR.ProductionId
		inner join #vw_Route_Origin_Time_To_Scale as vwTime on vwTime.RouteId = PR.RouteId
		inner join #vw_Route_Scale_Destination as vw on vw.RouteId = PR.RouteId
		inner join (SELECT ProductionId as id, sum(cast(vw.Value as int)) as NWagon,
		sum(cast(Cleaning as int)) as Cleaning, sum(cast(Final as int)) as Final,
		count(*) as ct
		  FROM [Route].[dbo].[vw_Tag_Overturned_Wagons_Instrument] as vw	
		  inner join OxD on OxD.OriginId = vw.LocationId
		  inner join rRouteOxD on rRouteOxD.OxDId = OxD.Id
		  inner join rProductionRoute on rProductionRoute.RouteId = rRouteOxD.Id and Final = 0
		  group by ProductionId) as SumNWagon on SumNWagon.Id = Production.Id
		where
		Production.Final = 0 and SumNWagon.Final = ct and SumNWagon.Cleaning = 0 and rProductionStock.Load > 0 
	-----------Fim

	-----------Inicio: Atualiza a Data e hora inicial quando todos os VVs do destino tiver 0 vagões virados
		UPDATE [dbo].[Production]
		SET dhi = getdate()
		from Production
		inner join rProductionStock  on Production.Id = rProductionStock.Id
		inner join [rProductionRoute] as PR on rProductionStock.Id = PR.ProductionId
		inner join #vw_Route_Origin_Time_To_Scale as vwTime on vwTime.RouteId = PR.RouteId
		inner join #vw_Route_Scale_Destination as vw on vw.RouteId = PR.RouteId
		inner join (SELECT ProductionId as id, sum(cast(vw.Value as int)) as NWagon,
		sum(cast(Cleaning as int)) as Cleaning, sum(cast(Final as int)) as Final,
		count(*) as ct
		  FROM [Route].[dbo].[vw_Tag_Overturned_Wagons_Instrument] as vw	
		  inner join OxD on OxD.OriginId = vw.LocationId
		  inner join rRouteOxD on rRouteOxD.OxDId = OxD.Id
		  inner join rProductionRoute on rProductionRoute.RouteId = rRouteOxD.Id and Final = 0
		  group by ProductionId) as SumNWagon on SumNWagon.Id = Production.Id
		where
		Production.Final = 0 and SumNWagon.NWagon = 0 and rProductionStock.Load = 0 
		and SumNWagon.Cleaning = 0 and SumNWagon.Final = 0
	
	-----------Fim


	-----------Inicio: Atualiza o initial load do stock se for null
		UPDATE [dbo].[rProductionStock]
		SET InitialLoad = vw.Value, 
		InstrumentId = vw.InstrumentId
		from rProductionStock
		inner join Production on Production.Id = rProductionStock.Id
		inner join [rProductionRoute] as PR on rProductionStock.Id = PR.ProductionId
		inner join #vw_Route_Origin_Time_To_Scale as vwTime on vwTime.RouteId = PR.RouteId
		inner join #vw_Route_Scale_Destination as vw on vw.RouteId = PR.RouteId
		where
		rProductionStock.InitialLoad is null	
	-----------Fim

	-----------Inicio: Amazena o valor inicial do totalizador quando todos os VVs do destino tiver 0 vagões virados
		UPDATE [dbo].[rProductionStock]
		SET InitialLoad = vw.Value, 
		InstrumentId = vw.InstrumentId
		from rProductionStock
		inner join Production on Production.Id = rProductionStock.Id
		inner join [rProductionRoute] as PR on rProductionStock.Id = PR.ProductionId
		inner join #vw_Route_Origin_Time_To_Scale as vwTime on vwTime.RouteId = PR.RouteId
		inner join #vw_Route_Scale_Destination as vw on vw.RouteId = PR.RouteId
		inner join (SELECT ProductionId as id, sum(cast(vw.Value as int)) as NWagon,
		sum(cast(Cleaning as int)) as Cleaning, sum(cast(Final as int)) as Final,
		count(*) as ct
		  FROM [Route].[dbo].[vw_Tag_Overturned_Wagons_Instrument] as vw	
		  inner join OxD on OxD.OriginId = vw.LocationId
		  inner join rRouteOxD on rRouteOxD.OxDId = OxD.Id
		  inner join rProductionRoute on rProductionRoute.RouteId = rRouteOxD.Id and Final = 0
		  group by ProductionId) as SumNWagon on SumNWagon.Id = Production.Id
		where
		Production.Final = 0 and SumNWagon.NWagon = 0 and rProductionStock.Load = 0 
		and SumNWagon.Cleaning = 0 and SumNWagon.Final = 0
	
	-----------Fim

	-----------Inicio: Destruindo tabelas temporarias
		drop table #vw_Route_Scale_Destination
		drop table #vw_Route_Origin_Time_To_Scale
	-----------Fim
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Queue_OxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Cleber Ferreira; Davi Sartori
-- Create date: <Create Date,,>
-- Description:	OxD
-- =============================================
CREATE PROCEDURE [dbo].[sp_Queue_OxD]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
/****** Adiciona a fila de OxD uma rota nova  ******/
	UPDATE [dbo].[rLocationValue]
	   SET [ValueFloat] = NewQueue.Value
	from rLocationValue
	inner join
	(	select LocationValueId, OxD.ValueFloat as Value from
		(
			SELECT ROW_NUMBER () over (order by ValueFloat asc) Id, ValueFloat from
			(select distinct ValueFloat
			FROM [Route].[dbo].[rRouteActive]
			--inner join rRouteOxD on rRouteOxD.LocationId = rRouteActive.Id
			inner join vw_RouteOxD as vwod on vwod.Id = rRouteActive.Id
			inner join OxD on OxD.DestinationId = vwod.DestinationId and OxD.OriginId = vwod.OriginId
			inner join Location as LOxD on LOxD.Id = OxD.Id
			inner join Location as L1 on L1.ParentId = LOxD.Id and L1.TypeId = 52
			inner join rLocationValue as LVal on LVal.Id = L1.Id
			left outer join [Route].[dbo].[vw_Queue_OxD] as vw on vw.Idx = LVal.ValueFloat
			where
			vw.Id is null
			) as Sub
		) as OxD 
		cross apply
		(
			SELECT ROW_NUMBER () over (order by Q.Id asc) as [Id], Idx, ValueFloat, q.Id as LocationValueId
			FROM [Route].[dbo].[vw_Queue_OxD] as Q
			inner join rLocationValue as LVal on LVal.Id = Q.Id
			where
			Idx is null
		) 
		as Q
		where
		Q.Idx is null and Q.Id = OxD.Id 
	) as NewQueue on NewQueue.LocationValueId = rLocationValue.Id

/****** Remove a fila de OxD uma rota removida  ******/
	UPDATE [dbo].[rLocationValue]
	   SET [ValueFloat] = null
	from rLocationValue
	inner join
	(
		SELECT ROW_NUMBER () over (order by Q.Id asc) as [Id], Idx, q.Id as LocationValueId
		FROM [Route].[dbo].[vw_Queue_OxD] as Q
		where
		Idx is not null
	) 
	as Q on Q.LocationValueId = rLocationValue.Id
	left outer join
	(
		SELECT ROW_NUMBER () over (order by ValueFloat asc) Id, ValueFloat from
			(select distinct ValueFloat
			FROM [Route].[dbo].[rRouteActive]
			inner join vw_RouteOxD as vwod on vwod.Id = rRouteActive.Id
			inner join OxD on OxD.DestinationId = vwod.DestinationId and OxD.OriginId = vwod.OriginId
			inner join Location as LOxD on LOxD.Id = OxD.Id
			inner join Location as L1 on L1.ParentId = OxD.Id and L1.TypeId = 52
			inner join rLocationValue as LVal on LVal.Id = L1.Id
			) as Sub
	) as OxD on OxD.ValueFloat = Q.Idx
	where
	OxD.Id is null

	exec sp_Tag_Refresh_OxD
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Route_Exec_OxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Davi Sartori
-- Create date: 10/06/2025
-- Description:	Stored Procedure que executa as ações do OxD
-- =============================================
CREATE PROCEDURE [dbo].[sp_Route_Exec_OxD]
AS
BEGIN
	exec sp_Queue_OxD
	exec sp_Tag_Refresh_OxD
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Route_Exec_Rules]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Route_Exec_Rules]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	exec sp_Route_Tag_Bool_Activate
	exec sp_Tag_Deactivate_Permission
	--exec sp_Queue_OxD
	--exec sp_Tag_Refresh_OxD
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Route_Tag_Bool_Activate]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Route_Tag_Bool_Activate]

AS
BEGIN
	UPDATE [dbo].[rTagWrite]
	SET [Write] = 1
	,[Value] = '1'
	FROM rTagWrite
	inner join [Route].[dbo].[vw_Tag_Bool_Activate] as vw on vw.TagId = rTagWrite.Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPV_Exec_Route_CNs]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Davi Sartori, Cleber Ferreira
-- Create date: 27/01/2025
-- Description:	Atualiza as rotas nos supervisórios dos operadores dos Carregadores de Navio
-- =============================================
CREATE PROCEDURE [dbo].[sp_SPV_Exec_Route_CNs]
AS
BEGIN
Update rtagWrite 
Set
Value = Route, Write = 1
--select *
from rTagWrite
inner join
(
select rRouteGraphLocation.LocationId as RouteId,Route, ValueFloat, TagId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 74 or rRouteGraphSequence.LocationId = 73 or rRouteGraphSequence.LocationId = 72 or rRouteGraphSequence.LocationId = 71)
inner join Location as Str on Str.ParentId = rRouteGraphSequence.LocationId and Str.TypeId = 65
inner join Location as Ins on Ins.ParentId = str.id and Ins.TypeId = 4
inner join rInstrumentMeasure as meas on meas.Id = Ins.Id
inner join rLocationValue as rlv on rlv.Id = meas.Id
inner join Tag on Tag.Id = meas.TagId
) as Tags on Tags.TagId = rTagWrite.Id
inner join
(
select ROW_NUMBER() OVER(PARTITION BY rRouteGraphSequence.LocationId ORDER BY rRouteGraphLocation.LocationId ASC) AS OrId, rRouteGraphLocation.LocationId as RouteId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 74 or rRouteGraphSequence.LocationId = 73 or rRouteGraphSequence.LocationId = 72 or rRouteGraphSequence.LocationId = 71)
) as OrRoute on OrRoute.RouteId = Tags.RouteId and OrRoute.OrId = tags.ValueFloat

END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPV_Exec_Route_Deactivate]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Davi Sartori, Cleber Ferreira
-- Create date: 27/01/2025
-- Description:	Atualiza as rotas nos supervisórios dos operadores dos Carregadores de Navio
-- =============================================
CREATE PROCEDURE [dbo].[sp_SPV_Exec_Route_Deactivate]
AS
BEGIN
	Update rtagWrite 
	Set
	Value = '', Write = 1
	--select *
	from rTagWrite
	inner join rInstrumentMeasure as meas on meas.TagId = rTagWrite.Id
	inner join rLocationValue as rlv on rlv.Id = meas.Id
	inner join Location as ins on ins.Id = meas.Id
	inner join Location as Str on Str.Id = ins.ParentId and Str.TypeId = 65
	left outer join
	(select Tag.Id from
	[Route].[dbo].[vw_Eqp_RouteActive] as vw
	inner join Location as Str on Str.ParentId = vw.Id and Str.TypeId = 65
	inner join Location as Ins on Ins.ParentId = str.id and Ins.TypeId = 4
	inner join rInstrumentMeasure as meas on meas.Id = Ins.Id
	inner join rLocationValue as rlv on rlv.Id = meas.Id
	inner join Tag on Tag.Id = meas.TagId) as TagInRoute on TagInRoute.Id = rTagWrite.Id
	where
	TagInRoute.Id is null
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPV_Exec_Route_EPs]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Davi Sartori Ferreira
-- Create date: 31/01/2025
-- Description:	Atualiza tags que mostram no Display dos operadores das EPs
-- =============================================
CREATE PROCEDURE [dbo].[sp_SPV_Exec_Route_EPs]
AS
BEGIN
Update rtagWrite 
Set
Value = Route, Write = 1
--select *
from rTagWrite
inner join
(
select rRouteGraphLocation.LocationId as RouteId,Route, ValueFloat, TagId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 104 or rRouteGraphSequence.LocationId = 105 or rRouteGraphSequence.LocationId = 106)
inner join Location as Str on Str.ParentId = rRouteGraphSequence.LocationId and Str.TypeId = 65
inner join Location as Ins on Ins.ParentId = str.id and Ins.TypeId = 4
inner join rInstrumentMeasure as meas on meas.Id = Ins.Id
inner join rLocationValue as rlv on rlv.Id = meas.Id
inner join Tag on Tag.Id = meas.TagId
) as Tags on Tags.TagId = rTagWrite.Id
inner join
(
select ROW_NUMBER() OVER(PARTITION BY rRouteGraphSequence.LocationId ORDER BY rRouteGraphLocation.LocationId ASC) AS OrId, rRouteGraphLocation.LocationId as RouteId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 104 or rRouteGraphSequence.LocationId = 105 or rRouteGraphSequence.LocationId = 106)
) as OrRoute on OrRoute.RouteId = Tags.RouteId and OrRoute.OrId = tags.ValueFloat
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SPV_Exec_Route_RCs]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Davi Sartori Ferreira
-- Create date: 29/01/2025
-- Description:	Atualiza as tags que aparecem no supervisório dos operadores das Recuperadoras
-- =============================================
CREATE PROCEDURE [dbo].[sp_SPV_Exec_Route_RCs]
AS
BEGIN
Update rtagWrite 
Set
Value = Route, Write = 1
--select *
from rTagWrite
inner join
(
select rRouteGraphLocation.LocationId as RouteId,Route, ValueFloat, TagId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 189 or rRouteGraphSequence.LocationId = 188 
or rRouteGraphSequence.LocationId = 187 or rRouteGraphSequence.LocationId = 186 or rRouteGraphSequence.LocationId = 184 or rRouteGraphSequence.LocationId = 191 or rRouteGraphSequence.LocationId = 192
or rRouteGraphSequence.LocationId = 193 or rRouteGraphSequence.LocationId = 104 or rRouteGraphSequence.LocationId = 105 or rRouteGraphSequence.LocationId = 106)
inner join Location as Str on Str.ParentId = rRouteGraphSequence.LocationId and Str.TypeId = 65
inner join Location as Ins on Ins.ParentId = str.id and Ins.TypeId = 4
inner join rInstrumentMeasure as meas on meas.Id = Ins.Id
inner join rLocationValue as rlv on rlv.Id = meas.Id
inner join Tag on Tag.Id = meas.TagId
) as Tags on Tags.TagId = rTagWrite.Id
inner join
(
select ROW_NUMBER() OVER(PARTITION BY rRouteGraphSequence.LocationId ORDER BY rRouteGraphLocation.LocationId ASC) AS OrId, rRouteGraphLocation.LocationId as RouteId
from rRouteActive
inner join rRouteGraphLocation on rRouteGraphLocation.LocationId = rRouteActive.Id
inner join RouteGraph on  RouteGraph.Id = rRouteGraphLocation.id
inner join rRouteGraphSequence on rRouteGraphSequence.RouteGraphId = RouteGraph.Id and (rRouteGraphSequence.LocationId = 189 or rRouteGraphSequence.LocationId = 188 or rRouteGraphSequence.LocationId = 187 
or rRouteGraphSequence.LocationId = 186 or rRouteGraphSequence.LocationId = 184 or rRouteGraphSequence.LocationId = 191 or rRouteGraphSequence.LocationId = 192 or rRouteGraphSequence.LocationId = 193 or rRouteGraphSequence.LocationId = 104
or rRouteGraphSequence.LocationId = 105 or rRouteGraphSequence.LocationId = 106)
) as OrRoute on OrRoute.RouteId = Tags.RouteId and OrRoute.OrId = tags.ValueFloat
where Route not like '%cn04%' and Route not like '%cn02a%'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Tag_Calc_Media_Driver]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Cleber Ferreira, Davi Sartori
-- Create date: 20/03/2025
-- Description: Calcula o tempo de execução de cada Driver
-- =============================================
CREATE PROCEDURE [dbo].[sp_Tag_Calc_Media_Driver]
AS
BEGIN
update rLocationValue set rLocationValue.ValueInt = rLocationValue.ValueInt + 1, rLocationValue.ValueDateTime = DATEADD(Second, DATEDIFF(second, LastDh, Dh) + DATEDiff(second,'2025-01-01', rLocationValue.ValueDateTime), '2025-01-01')
--select *
from rLocationValue
	inner join 
		(select max(nome.dh) as dh, min(nome.LastDh) as LastDh, nome.Id
		from rLocationValue
		inner join (select distinct	rLocationValue.Id, rLocationValue.ValueInt, rLocationValue.ValueDateTime, meas.dh, meas.LastDh from rLocationValue
		inner join Location as LValue on LValue.Id = rLocationValue.Id
		inner join Location as Plc on Plc.Id = LValue.Parentid
		inner join Tag on Tag.PlcId = Plc.Id
		inner join rTagGroup as tg on tg.TagId = Tag.Id
		inner join rInstrumentMeasure as meas on meas.TagId = Tag.Id
		group by rLocationValue.Id, rLocationValue.ValueInt, rLocationValue.ValueDateTime, meas.dh, meas.LastDh) as nome on nome.id = rLocationValue.Id
		group by nome.Id) as T1 on T1.Id = rLocationValue.Id

update rLocationValue
set ValueDateTime = '2025-01-01'
,ValueInt = 0
where DATEDIFF(MINUTE, '2025-01-01', rLocationValue.ValueDateTime) > 29

insert into dbo.[log]
(ApplicationId
,dh
,Message
,LocationId)
select 2, getdate(), 'Travamento de Driver de Comunicação', 1 from vw_Tag_Media_Driver
where Expr1 like '%Bad%'

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Tag_Deactivate_Permission]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Tag_Deactivate_Permission]

AS
BEGIN

	UPDATE [dbo].[rTagWrite]
	SET [Write] = 1
	,[Value] = '0'
	FROM rTagWrite
	inner join [Route].[dbo].[vw_Tag_Permission_Deactivate] as vw on vw.TagId = rTagWrite.Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Tag_Refresh_OxD]    Script Date: 09/09/2026 06:34:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Tag_Refresh_OxD]

AS
BEGIN
	UPDATE [dbo].[rTagWrite]
	SET [Write] = 1
	,[Value] = case when vw.Idx = 137 then 0
				else ISNULL(idx,0) end
	FROM rTagWrite
	inner join [Route].[dbo].[vw_Queue_OxD] as vw on vw.TagId = rTagWrite.Id

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Eqp"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Meas"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 119
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 120
               Left = 662
               Bottom = 250
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
En' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Eqp_FinalProduction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'd
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Eqp_FinalProduction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Eqp_FinalProduction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "active"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rgl"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 102
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Seq"
            Begin Extent = 
               Top = 6
               Left = 486
               Bottom = 136
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Eqp_RouteActive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Eqp_RouteActive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Production_Rateio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Production_Rateio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[29] 4[4] 2[49] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rLocationValue"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Queue_OxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Queue_OxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Queue_OxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Route"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Queue_OxD_Route'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Queue_OxD_Route'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Q"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tableoftrue"
            Begin Extent = 
               Top = 6
               Left = 262
               Bottom = 136
               Right = 448
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Damper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Damper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[5] 2[61] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Feeder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Feeder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[13] 4[5] 2[65] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Reversal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Reversal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[5] 2[62] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Rule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[6] 4[15] 2[63] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Tripper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Consistency_Tripper'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Log"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 206
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "active"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "seq"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tr"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Bal"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "BalTime"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 232
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Time"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Tabl' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Origin_Time_To_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'e = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Origin_Time_To_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Origin_Time_To_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rra"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 152
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rrgl"
            Begin Extent = 
               Top = 9
               Left = 615
               Bottom = 152
               Right = 837
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rrgs"
            Begin Extent = 
               Top = 9
               Left = 894
               Bottom = 206
               Right = 1116
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "loc"
            Begin Extent = 
               Top = 9
               Left = 1173
               Bottom = 206
               Right = 1395
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 13
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
         Or ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Rule_OldArea'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Rule_OldArea'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Rule_OldArea'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[1] 2[40] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rRouteActive"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Route"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LocR"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "seq"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L1"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 232
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 102
               Left = 246
               Bottom = 232
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L3"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            TopColumn ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 0
         End
         Begin Table = "InsVal"
            Begin Extent = 
               Top = 138
               Left = 662
               Bottom = 251
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 234
               Left = 38
               Bottom = 364
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale_Destination'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale_Destination'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Bal"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Seq"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrderBal"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 102
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LO"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LBAL"
            Begin Extent = 
               Top = 102
               Left = 454
               Bottom = 232
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   E' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale_Origin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'nd
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale_Origin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Scale_Origin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[4] 2[46] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_CMD"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_REF"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 251
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_INS"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 222
               Left = 662
               Bottom = 352
               Right = 832
            End
            DisplayFlags = 280
            Top' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Column = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 252
               Left = 38
               Bottom = 348
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_CMD"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_REF"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 251
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DP_INS"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 222
               Left = 662
               Bottom = 352
               Right = 832
            End
            DisplayFlags = 280
            To' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'pColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 252
               Left = 38
               Bottom = 348
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct2"
            Begin Extent = 
               Top = 270
               Left = 246
               Bottom = 366
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Damper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RGL"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SEQ"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L1"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L2"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 232
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_PERM"
            Begin Extent = 
               Top = 102
               Left = 246
               Bottom = 181
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L3"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            T' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_D_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'opColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 138
               Left = 662
               Bottom = 268
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 186
               Left = 246
               Bottom = 316
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 234
               Left = 38
               Bottom = 364
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_D_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_D_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RGL"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SEQ"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L1"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L2"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 232
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_PERM"
            Begin Extent = 
               Top = 102
               Left = 246
               Bottom = 181
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L3"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            T' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_O_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'opColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 138
               Left = 662
               Bottom = 268
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 186
               Left = 246
               Bottom = 316
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 234
               Left = 38
               Bottom = 364
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_O_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_O_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RGL"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SEQ"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 136
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L1"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_PERM"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 181
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L2"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            Top' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Column = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 186
               Left = 38
               Bottom = 316
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 270
               Left = 246
               Bottom = 400
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Eqp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CM"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CM_L1"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD_REF"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REFVAL"
            Begin Extent = 
               Top = 120
               Left = 870
               Bottom = 233
               Right = 1045
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD_INS"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'mn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 119
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1078
               Bottom = 136
               Right = 1248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 6
               Left = 1286
               Bottom = 136
               Right = 1456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 234
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[33] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CM"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CM_L1"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD_REF"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REFVAL"
            Begin Extent = 
               Top = 120
               Left = 870
               Bottom = 233
               Right = 1045
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD_INS"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'mn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 119
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1078
               Bottom = 136
               Right = 1248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 102
               Left = 1286
               Bottom = 232
               Right = 1456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 234
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct2"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 234
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Feeder_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RGL"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 198
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RX"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L1"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_PERM"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L2"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 136
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 6
               Left = 1078
               Bottom = 119
               Right = 1248
            End
            DisplayFlags = 280
            TopCol' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_OxD_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'umn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 6
               Left = 1286
               Bottom = 136
               Right = 1456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_OxD_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_OxD_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 119
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1078
               Bottom = 136
               Right = 1248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 6
               Left = 1286
               Bottom = 136
               Right = 1456
            End
            DisplayFlags = 280
            TopColu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'mn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 186
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 119
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1078
               Bottom = 136
               Right = 1248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'n = 0
         End
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 1286
               Bottom = 102
               Right = 1456
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 234
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct2"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 234
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Reversal_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L1"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 136
               Right = 426
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L2"
            Begin Extent = 
               Top = 6
               Left = 464
               Bottom = 136
               Right = 634
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 672
               Bottom = 85
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 880
               Bottom = 119
               Right = 1050
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1088
               Bottom = 136
               Right = 1258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 6
               Left = 1296
               Bottom = 136
               Right = 1466
            End
            DisplayFlags = 280
            TopColu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'mn = 0
         End
         Begin Table = "REF_OK"
            Begin Extent = 
               Top = 90
               Left = 672
               Bottom = 220
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CON"
            Begin Extent = 
               Top = 120
               Left = 880
               Bottom = 250
               Right = 1050
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_ALL"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 234
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Command'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[20] 2[25] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 218
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L1"
            Begin Extent = 
               Top = 6
               Left = 256
               Bottom = 136
               Right = 426
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "REF_L2"
            Begin Extent = 
               Top = 6
               Left = 464
               Bottom = 136
               Right = 634
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CMD"
            Begin Extent = 
               Top = 6
               Left = 672
               Bottom = 85
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS_OK"
            Begin Extent = 
               Top = 6
               Left = 880
               Bottom = 119
               Right = 1050
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TAG_OK"
            Begin Extent = 
               Top = 6
               Left = 1088
               Bottom = 136
               Right = 1258
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 6
               Left = 1296
               Bottom = 136
               Right = 1466
            End
            DisplayFlags = 280
            TopColu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'mn = 0
         End
         Begin Table = "EqpAct"
            Begin Extent = 
               Top = 90
               Left = 672
               Bottom = 186
               Right = 842
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EqpAct2"
            Begin Extent = 
               Top = 120
               Left = 880
               Bottom = 216
               Right = 1050
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_Tag_Tripper_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Log"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 206
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_View_Historycal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_View_Historycal'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ACTIVE"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 102
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RGL"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 102
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SEQ"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ROUTE"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 119
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L1"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 136
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_L2"
            Begin Extent = 
               Top = 102
               Left = 38
               Bottom = 232
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EQP_PERM"
            Begin Extent = 
               Top = 102
               Left = 246
               Bottom = 181
               Right = 416
            End
            DisplayFlags = 280
            Top' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_VV_Limp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Column = 0
         End
         Begin Table = "EQP_L3"
            Begin Extent = 
               Top = 120
               Left = 662
               Bottom = 250
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 268
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 138
               Left = 870
               Bottom = 268
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 186
               Left = 246
               Bottom = 316
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_VV_Limp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Route_VV_Limp_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Log"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 7
               Left = 290
               Bottom = 170
               Right = 484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_RouteLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_RouteLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rgmn"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location_1"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Origin_1"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "mmr"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 203
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rgl"
            Begin Extent = 
               Top = 120
               Left = 38
               Bottom = 216
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "rgmx"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "act"
            Begin Extent = 
               Top = 138
               Left = 454
               Bottom = 234
               Right = 624
            End
            DisplayFlags = 280
            Top' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_RouteOxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Column = 0
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 6
               Left = 870
               Bottom = 136
               Right = 1040
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cto"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_RouteOxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_RouteOxD'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rRouteActive"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 152
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_Pier2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_Pier2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rRouteActive"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 152
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Location"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_RC04'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_RC04'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rra"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 152
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "loc"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_RC05'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Spv_Rotascco_RC05'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[11] 4[4] 2[67] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Bool_Activate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Bool_Activate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Location"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Dismember_Instrument'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Dismember_Instrument'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rLocationValue"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 206
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "T1"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 179
               Right = 558
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Media_Driver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Media_Driver'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[57] 4[16] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "L1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Eqp"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "InsVal"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 119
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 120
               Left = 662
               Bottom = 250
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Plc"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 11' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Overturned_Wagons_Instrument'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'70
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Overturned_Wagons_Instrument'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Overturned_Wagons_Instrument'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LP"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L1"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "L2"
            Begin Extent = 
               Top = 6
               Left = 454
               Bottom = 136
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "I"
            Begin Extent = 
               Top = 6
               Left = 662
               Bottom = 85
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MEAS"
            Begin Extent = 
               Top = 90
               Left = 662
               Bottom = 220
               Right = 832
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Tag"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PLC"
            Begin Extent = 
               Top = 138
               Left = 246
               Bottom = 268
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Permission'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[5] 2[50] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Act"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 85
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Permission_Deactivate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Tag_Permission_Deactivate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[16] 4[24] 2[44] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwRateioGpvDescPublish'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwRateioGpvDescPublish'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vwRateioGpvDescPublish"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwRateioHistoryDescPublish'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwRateioHistoryDescPublish'
GO
