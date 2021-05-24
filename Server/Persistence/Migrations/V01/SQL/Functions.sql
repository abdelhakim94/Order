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


-- -----------------------------------------------------------------------------
-- User categories function
-- -----------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION order_schema.user_categories(id_user integer)
 RETURNS TEXT
 LANGUAGE plpgsql RETURNS NULL ON NULL INPUT
AS $function$
DECLARE
	result TEXT;
BEGIN
	SELECT STRING_AGG(label, ', ') INTO result FROM
	(
			SELECT DISTINCT(c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_dish cd
			ON c.id = cd.id_card
			INNER JOIN order_schema.dish d
			ON cd.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true
		UNION
			SELECT DISTINCT (c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_menu cm
			ON c.id = cm.id_card
			INNER JOIN order_schema.menu m
			ON cm.id_menu = m.id
			INNER JOIN order_schema.menu_dish md
			ON m.id = md.id_menu
			INNER JOIN order_schema.dish d
			ON md.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true
		UNION
			SELECT DISTINCT (c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_menu cm
			ON c.id = cm.id_card
			INNER JOIN order_schema.menu m
			ON cm.id_menu = m.id
			INNER JOIN order_schema.menu_section ms
			ON m.id = ms.id_menu
			INNER JOIN order_schema.section s
			ON ms.id_section = s.id
			INNER JOIN order_schema.dish_section ds
			ON s.id = ds.id_section
			INNER JOIN order_schema.dish d
			ON ds.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true AND ms.menu_owns =true
		UNION
			SELECT DISTINCT(c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_section cs
			ON c.id = cs.id_card
			INNER JOIN order_schema.section s
			ON cs.id_section = s.id
			INNER JOIN order_schema.dish_section ds
			ON s.id = ds.id_section
			INNER JOIN order_schema.dish d
			ON ds.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true
		UNION
			SELECT DISTINCT(c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_section cs
			ON c.id = cs.id_card
			INNER JOIN order_schema.section s1
			ON cs.id_section = s1.id
			INNER JOIN order_schema.menu_section ms1
			ON s1.id = ms1.id_section
			INNER JOIN order_schema.menu m
			ON ms1.id_menu = m.id
			INNER JOIN order_schema.menu_section ms2
			ON m.id = ms2.id_menu
			INNER JOIN order_schema.section s2
			ON ms2.id_section = s2.id
			INNER JOIN order_schema.dish_section ds
			ON s2.id = ds.id_section
			INNER JOIN order_schema.dish d
			ON ds.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true AND ms1.menu_owns = false and ms2.menu_owns = true
		UNION
			SELECT DISTINCT(c1.label) AS label
			FROM order_schema.user u
			INNER JOIN order_schema.card c
			ON u."Id" = c.id_user
			INNER JOIN order_schema.card_section cs
			ON c.id = cs.id_card
			INNER JOIN order_schema.section s
			ON cs.id_section = s.id
			INNER JOIN order_schema.menu_section ms
			ON s.id = ms.id_section
			INNER JOIN order_schema.menu m
			ON ms.id_menu = m.id
			INNER JOIN order_schema.menu_dish md
			ON m.id = md.id_menu
			INNER JOIN order_schema.dish d
			ON md.id_dish = d.id
			INNER JOIN order_schema.dish_category dc
			ON d.id = dc.id_dish
			INNER JOIN order_schema.category c1
			ON dc.id_category = c1.id
			WHERE u."Id" = $1 AND c.is_active = true AND ms.menu_owns = false
	) t;

	RETURN result;
	 
END;
$function$
;
