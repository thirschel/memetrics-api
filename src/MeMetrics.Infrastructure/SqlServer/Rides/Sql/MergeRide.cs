namespace MeMetrics.Infrastructure.SqlServer.Rides.Sql
{
    public class MergeRide
    {
        public const string Value = @"
                        MERGE INTO [dbo].[Ride] AS TARGET
                        USING (SELECT
                             @RideId AS RideId
                            ,@RideType AS RideType
                            ,@OriginLat AS OriginLat
                            ,@OriginLong AS OriginLong
                            ,@DestinationLat AS DestinationLat
                            ,@DestinationLong AS DestinationLong
                            ,@RequestDate AS RequestDate
                            ,@PickupDate AS PickupDate
                            ,@DropoffDate AS DropoffDate
                            ,@Price AS Price
                            ,@Distance AS Distance)
                        AS SOURCE 
                        ON TARGET.RideId = SOURCE.RideId 
                        WHEN MATCHED THEN
						UPDATE SET 
                            TARGET.OriginLat = SOURCE.OriginLat,
                            TARGET.OriginLong = SOURCE.OriginLong,
                            TARGET.DestinationLat = SOURCE.DestinationLat,
                            TARGET.DestinationLong = SOURCE.DestinationLong,
                            TARGET.RequestDate = SOURCE.RequestDate,
                            TARGET.PickupDate = SOURCE.PickupDate,
                            TARGET.DropoffDate = SOURCE.DropoffDate,
                            TARGET.Price = SOURCE.Price,
                            TARGET.Distance = SOURCE.Distance
                        WHEN NOT MATCHED THEN
                        INSERT 
                              (RideId
                              ,RideType
                              ,OriginLat
                              ,OriginLong
                              ,DestinationLat
                              ,DestinationLong
                              ,RequestDate
                              ,PickupDate
                              ,DropoffDate
                              ,Price
                              ,Distance)
                        VALUES (
                               SOURCE.RideId
                              ,SOURCE.RideType
                              ,SOURCE.OriginLat
                              ,SOURCE.OriginLong
                              ,SOURCE.DestinationLat
                              ,SOURCE.DestinationLong
                              ,SOURCE.RequestDate
                              ,SOURCE.PickupDate
                              ,SOURCE.DropoffDate
                              ,SOURCE.Price
                              ,SOURCE.Distance);";
    }
}
