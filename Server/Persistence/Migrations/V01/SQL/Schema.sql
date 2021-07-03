-- -----------------------------------------------------------------------------
-- CATEGORY
-- -----------------------------------------------------------------------------

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 1, 'Traditionnel', 'images/category/traditionnel.png', true, 1
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 1);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 2, 'Djouez et tadjines', 'images/category/djouez-et-tadjines.png', true, 2
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 2);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 3, 'Les pains', 'images/category/pains.png', true, 3
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 3);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 4, 'Soupes', 'images/category/soupes.png', true, 4
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 4);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 5, 'Riz et grains', 'images/category/riz-et-grains.png', true, 5
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 5);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 6, 'Pâtes', 'images/category/pates.png', true, 6
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 6);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 7, 'Pizza', 'images/category/pizza.png', true, 7
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 7);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 8, 'Salées', 'images/category/salees.png', true, 8
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 8);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 9, 'Sandwitches', 'images/category/sandwitches.png', true, 9
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 9);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 10, 'Salades', 'images/category/salades.png', true, 10
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 10);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 11, 'Viandes', 'images/category/viandes.png', true, 11
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 11);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 12, 'Poisson et fruits de mer', 'images/category/poisson-et-fruits-de-mer.png', true, 12
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 12);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 13, 'Desserts', 'images/category/desserts.png', true, 13
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 13);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 14, 'Boissons', 'images/category/boissons.png', true, 14
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 14);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 15, 'Pâtisseries', 'images/category/patisseries.png', true, 15
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 15);

INSERT INTO order_schema.category( id, label, picture, is_main, ordre) SELECT 16, 'Gâteaux', 'images/category/gateaux.png', true, 16
    WHERE NOT EXISTS (SELECT id FROM order_schema.category WHERE id = 16);

-- -----------------------------------------------------------------------------
-- WILAYA
-- -----------------------------------------------------------------------------

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  1, '01000', 'Adrar'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 1);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  2, '02000', 'Chlef'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 2);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  3, '03000', 'Laghouat'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 3);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  4, '04000', 'Oum El Bouaghi'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 4);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  5, '05000', 'Batna'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 5);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  6, '06000', 'Béjaïa'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 6);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  7, '07000', 'Biskra'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 7);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  8, '08000', 'Béchar'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 8);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  9, '09000', 'Blida'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 9);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  10, '10000', 'Bouira'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 10);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  11, '11000', 'Tamanrasset'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 11);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  12, '12000', 'Tébessa'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 12);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  13, '13000', 'Tlemcen'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 13);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  14, '14000', 'Tiaret'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 14);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  15, '15000', 'Tizi Ouzou'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 15);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  16, '16000', 'Alger'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 16);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  17, '17000', 'Djelfa'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 17);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  18, '18000', 'Jijel'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 18);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  19, '19000', 'Sétif'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 19);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  20, '20000', 'Saïda'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 20);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  21, '21000', 'Skikda'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 21);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  22, '22000', 'Sidi Bel Abbès'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 22);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  23, '23000', 'Annaba'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 23);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  24, '24000', 'Guelma'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 24);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  25, '25000', 'Constantine'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 25);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  26, '26000', 'Médéa'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 26);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  27, '27000', 'Mostaganem'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 27);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  28, '28000', 'M''Sila'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 28);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  29, '29000', 'Mascara'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 29);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  30, '30000', 'Ouargla'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 30);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  31, '31000', 'Oran'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 31);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  32, '32000', 'El Bayadh'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 32);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  33, '33000', 'Illizi'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 33);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  34, '34000', 'Bordj Bou Arreridj'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 34);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  35, '35000', 'Boumerdès'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 35);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  36, '36000', 'El Tarf'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 36);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  37, '37000', 'Tindouf'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 37);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  38, '38000', 'Tissemsilt'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 38);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  39, '39000', 'El Oued'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 39);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  40, '40000', 'Khenchela'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 40);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  41, '41000', 'Souk Ahras'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 41);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  42, '42000', 'Tipaza'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 42);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  43, '43000', 'Mila'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 43);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  44, '44000', 'Aïn Defla'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 44);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  45, '45000', 'Naâma'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 45);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  46, '46000', 'Aïn Témouchent'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 46);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  47, '47000', 'Ghardaïa'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 47);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  48, '48000', 'Relizane'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 48);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  49, '49000', 'Timimoun'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 49);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  50, '50000', 'Bordj Badji Mokhtar'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 50);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  51, '51000', 'Ouled Djellal'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 51);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  52, '52000', 'Béni Abbès'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 52);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  53, '53000', 'In Salah'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 53);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  54, '54000', 'In Guezzam'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 54);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  55, '55000', 'Touggourt'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 55);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  56, '56000', 'Djanet'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 56);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  57, '57000', 'El Meghaier'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 57);

INSERT INTO order_schema.wilaya( id, zip_code, name ) SELECT  58, '58000', 'El Menia'
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 58);

-- -----------------------------------------------------------------------------
-- CITY
-- -----------------------------------------------------------------------------

-- ALGER

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 1, 'Hussein Dey', 36.74122612764448, 3.100055899888601, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 1);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 2, 'Les Eucalyptus', 36.67264064932099, 3.169855592941146, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 2);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 3, 'Sidi Moussa', 36.61381706590312, 3.104636256420499, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 3);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 4, 'Kouba', 36.72608202659451, 3.0827564096533684, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 4);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 5, 'Mohamed Belouzdad', 36.75231598008026, 3.06732555290093, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 5);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 6, 'Ain Taya', 36.792398246202836, 3.2880353864857588, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 6);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 7, 'Bab Ezzouar', 36.72276427071924, 3.1878119703303605, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 7);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 8, 'Bordj El Kiffan', 36.760408153931174, 3.2331711347359064, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 8);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 9, 'Dar El Beida', 36.71825655988211, 3.21107780271295, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 9);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 10, 'El Marsa', 36.809657094100594, 3.2403158908532976, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 10);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 11, 'Mohammadia', 36.72946798712401, 3.1599888275002037, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 11);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 12, 'Bir Touta', 36.64105544926738, 2.995527091879014, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 12);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 13, 'Ouled Chebel', 36.59211793075703, 2.9894238277830447, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 13);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 14, 'Tessala El Merdja', 36.62800950169739, 2.9458150955978906, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 14);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 15, 'Herraoua', 36.771111327913964, 3.311953304736952, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 15);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 16, 'Reghaia', 36.74076277003622, 3.3427058163810446, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 16);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 17, 'Rouiba', 36.74366986815605, 3.27971343673624, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 17);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 18, 'Mahelma', 36.686267536018704, 2.8763790735200594, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 18);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 19, 'Rahmania', 36.68058806079273, 2.90803841717201, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 19);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 20, 'Souidania', 36.71003550542575, 2.911755616025952, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 20);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 21, 'Staoueli', 36.752339689112176, 2.888512572130441, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 21);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 22, 'Zeralda', 36.71605604761068, 2.843715677205853, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 22);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 23, 'Baba Hassen', 36.6931888083141, 2.9742210016374893, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 23);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 24, 'Douira', 36.67097673564836, 2.9505131434885215, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 24);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 25, 'Draria', 36.718409573299, 2.9977445968481486, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 25);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 26, 'El Achour', 36.738210189037694, 2.985423983902971, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 26);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 27, 'Khraissia', 36.66811147850793, 3.0000807432828163, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 27);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 28, 'Ain Benian', 36.80019647548792, 2.921025920673243, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 28);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 29, 'Cheraga', 36.75518388406338, 2.950402381488552, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 29);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 30, 'Dely Ibrahim', 36.76019454729815, 2.989356134586426, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 30);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 31, 'Hammamet', 36.81145708006819, 2.976027890926191, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 31);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 32, 'Ouled Fayet', 36.73445860968137, 2.93266093114632, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 32);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 33, 'Alger Centre', 36.77197491665898, 3.052915795088493, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 33);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 34, 'El Madania', 36.74289102677219, 3.0667637172336444, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 34);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 35, 'El Mouradia', 36.750090403893985, 3.0485400342053337, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 35);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 36, 'Sidi M''hamed', 36.758740741966776, 3.0541630249611758, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 36);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 37, 'Sehaoula', 36.7031476689661, 3.024046040680366, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 37);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 38, 'Bologhine Ibnou Ziri', 36.80405547818961, 3.0391162175756965, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 38);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 39, 'Casbah', 36.78465735446536, 3.058773299373445, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 39);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 40, 'Oued Koriche', 36.78228377827725, 3.043896378902689, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 40);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 41, 'Rais Hamidou', 36.81249805423281, 3.0102244599500474, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 41);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 42, 'Bir Mourad Rais', 36.73485196167318, 3.044691732539581, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 42);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 43, 'Birkhadem', 36.71663999505671, 3.046771981974298, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 43);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 44, 'Djasr Kasentina', 36.69644595610674, 3.0770931472844327, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 44);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 45, 'Hydra', 36.74495040586574, 3.0281680004537987, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 45);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 46, 'El Magharia', 36.7307867374265, 3.1112407879307296, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 46);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 47, 'Ben Aknoun', 36.75893723147174, 3.011056341375435, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 47);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 48, 'Beni Messous', 36.78042761739868, 2.9781083095874252, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 48);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 49, 'Bouzareah', 36.78795704086917, 3.010021227676068, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 49);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 50, 'El Biar', 36.76822147486566, 3.0332177745898568, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 50);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 51, 'Bachedjerah', 36.7247629550618, 3.1138643865011737, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 51);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 52, 'Bourouba', 36.71426909657578, 3.115070386669577, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 52);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 53, 'El Harrach', 36.721261049014466, 3.1393759286756784, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 53);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 54, 'Oued Smar', 36.70757670659599, 3.165494648980882, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 54);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 55, 'Baraki', 36.66866412356085, 3.102211890378535, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 55);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 56, 'Bordj El Bahri', 36.7901756014788, 3.249628644967253, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 56);

INSERT INTO order_schema.city( id, name, latitude, longitude, id_wilaya) SELECT 57, 'Bab El Oued', 36.79138356140047, 3.0528809685552583, 16
    WHERE NOT EXISTS(SELECT id FROM order_schema.wilaya WHERE id = 57);

-- -----------------------------------------------------------------------------
-- PROFILE
-- -----------------------------------------------------------------------------

INSERT INTO order_schema.profile( id, name ) SELECT 0, 'Admin' WHERE NOT EXISTS (SELECT id FROM order_schema.profile WHERE id = 0);
INSERT INTO order_schema.profile( id, name ) SELECT 1, 'Guest' WHERE NOT EXISTS (SELECT id FROM order_schema.profile WHERE id = 1);
INSERT INTO order_schema.profile( id, name ) SELECT 2, 'Delivery_man' WHERE NOT EXISTS (SELECT id FROM order_schema.profile WHERE id = 2);
INSERT INTO order_schema.profile( id, name ) SELECT 3, 'Chef' WHERE NOT EXISTS (SELECT id FROM order_schema.profile WHERE id = 3);

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
	) t;

	RETURN result;
	 
END;
$function$
;
