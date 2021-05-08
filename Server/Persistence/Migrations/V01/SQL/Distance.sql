-- -----------------------------------------------------------------------------
-- Distance calculation function
-- -----------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION order_schema.is_near(id_user integer, latitude numeric, longitude numeric, min_distance numeric)
 RETURNS boolean
 LANGUAGE plpgsql RETURNS NULL ON NULL INPUT
AS $function$
DECLARE
    R constant NUMERIC := 6371;
	u_latitude numeric;
	u_longitude numeric;
	a numeric;
	c numeric;
	deltaPhi numeric;
	deltaTheta numeric;
BEGIN
    SELECT c.latitude, c.longitude INTO u_latitude, u_longitude
	FROM order_schema.user_address AS ua
	INNER JOIN order_schema.city AS c
	ON ua.id_city = c.id
	WHERE ua.id_user = $1
	ORDER BY ua.last_time_used DESC
	LIMIT 1;
	
	IF u_latitude IS NULL OR u_longitude IS NULL THEN
		RETURN false;
	ELSE
		deltaPhi := RADIANS(latitude - u_latitude);
		deltaTheta := RADIANS(longitude - u_longitude);
		a := (SIN(deltaPhi/2)^2) + COS($2)*COS(u_latitude)*(SIN(deltaTheta/2)^2);
		c := 2 * ATAN2(|/a, |/(1-a));
		RETURN (R * c) <= min_distance;
	END IF;
END;
$function$
;
