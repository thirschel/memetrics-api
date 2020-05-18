namespace MeMetrics.Infrastructure.SqlServer.Rides.Sql
{
    public class GetRides
    {
        public const string Value = @"
                    SELECT 
                        RideId
                        ,RideType
                        ,OriginLat
                        ,OriginLong
                        ,DestinationLat
                        ,DestinationLong
                        ,RequestDate
                        ,PickupDate
                        ,DropoffDate
                        ,Price
                        ,Distance
                    FROM Call;";
    }
}
