CREATE DATABASE  IF NOT EXISTS `soft` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `soft`;
-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: soft
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` (id, role, first_name, last_name, email, password, salt, is_active, forgot_password_token, forgot_token_gen_date) VALUES (1,4,'admin','admin','admin.@gmail.com','b06bd722431d5794fb7fdc6a2c640294d8771a0f23ea2643c7d4e7d75f46d59a','V8W-2Xr1zW45kc9',1,NULL,NULL),(2,3,'Жанна','дАрк','JannaOnFire@gmail.com','eacbdc5e75850e7d07ac8504883c3de9a7225e93310417d079d53681d8a75eaa','AP4zYvi5JyR1t6NWbk1a',0,NULL,NULL),(3,3,'Taras','Nikolaev','Taras@gmail.comg','59ab6d0386bf6d4c41cb54561abf1b3b35a512e4f749d714fc11eb33613fd00b','AP4zYvi5JyR1t6NWbk1b',0,NULL,NULL),(4,2,'Фродо','Бэггинс','Frodo@gmail.com','a7fb1ad7840034a97564c1a834a0f95cbf376912e9e5ee48c1142ce8ef62676e','AP4zYvi5JyR1t6NWbk1R',0,NULL,NULL),(5,2,'Леголас','New','newEmail@gmail.com','19ba92491c216c84693a0a0f3309ec2785e8cb010c45a1c513b7608332a11a97','xgqjh8yPx6QvGjRRdhyQ',0,NULL,NULL),(6,2,'Торинопр','Дубощитпкп','Thorin@gmail.com','265c8d045e4debf7dc52586d8dcf33609825693bdffa7362102d49d7828b28cf','N0CLFg7Ts8CUFqN3wkvd',0,NULL,NULL),(7,1,'Сергейыевр','Мордор','sergMor@gmail.com','de6462d2bfc578327152278c298fcec9598265d15d423ebed6d7981a71f92d4d','wqfKvbJaBsYqRh7jVtQt',0,NULL,NULL),(8,1,'Егорffgh','Чародейazdryh','Egor@gmail.com','b97ad424aa93766ab97a7b9d5ee7e25b74c052139e6f7434c7486bf36d529712','5t5mFqxaLzpcacJ7x8on',0,NULL,NULL),(9,1,'Дмитро','Devil','Devil@gmail.com','6a0adca70bae18b92a55fd663483c268990ab86ff94012472948f7be8b13768e','FsDexnYG6uJp78Et4sXd',0,NULL,NULL),(10,1,'Liza','Klymenko','Klymenko@gmail.com','a244b8814de3250ecc406e39cdc7ed2e21f52600d45ed098d368aad608409c65','m3VM8tTUzPMd3LcUKRdn',0,NULL,NULL),(11,1,'Голлумм','Смеагол','ILoveRings@gmail.com','903a9ceb5d34245eefd388b490d18e9f2cb4dad7337600ae9ca6f99d5ae230f0','LjvYkDTqWNQ4vP0qqpud',1,NULL,NULL),(12,1,'Гюнтер','Дим','God@gmail.com','5dcafa351b64375e41f63f3efbcb19b553380837796427c1f3599af0b1d32f6c','bhk4KfVdXJM28A2TGeGd',0,NULL,NULL),(13,1,'Кейра','Мец','Keira.@gmail.com','1817ab380941708ed5b5e153685f147a37f36026127134c710f7f66bc2ff1876','Z45GC6Rn8EDcsXXtCiVa',0,NULL,NULL),(14,1,'Трисс','Меригольд','Triss@gmail.com','b5f2143ce4a27109711ba8d65920281b47dbceb811c98b974a15a27443046fff','YMvGWFQ0q38L8P7ZXCpJ',0,NULL,NULL),(15,1,'Вернон','Роше','Vernon.@gmail.com','f18d409f970a766f242ba0e789d6e9e51e5c629794cce6411ec6c96456c907c4','YQTyrEKEE28WCktWdMPQ',0,NULL,NULL),(16,1,'Эредин','БреаккГлас','Eredin@gmail.com','d5d88a32b44cb1a47f9cf823a198b78100ad3ad90775b36bb338eca1ad2590c7','V5XeFuFBXDoQss1pn3oT',0,NULL,NULL),(17,1,'Хагрид','Рубеус','Hagrid@gmail.com','2136d4f71fbd73c9eda40ee9262c6a9b4efc7895d7c297e8d62b05bad3ca0a47','LcJmgsMD886diEM99w7v',1,NULL,NULL),(18,1,'Полумна','Лавгуд','Luna.@gmail.com','a329a64ab7c623089d1947a5a0ebce04463cfaaf131874b4c4ede03f3aab3002','eZA4zvb4EBQgMY2rW6rX',1,NULL,NULL),(19,1,'Минерва','Макгонагалл','McGonagall.@gmail.com','5298ba2960215a4aeecbbaeae3949a2b0a840410a20006c52f27edf6a94bca42','wmYwW02uijgPe2FzR2Xs',1,NULL,NULL),(20,1,'Чжоу','Чанг','Chang@gmail.com','20b31894a220f9e9c2bee58f9628fffcfb32ee3ad480fc6da0032bc952642e53','9DHgFtVENMLeGAdg7hqU',0,NULL,NULL),(21,1,'Северус','Снегг','Snape@gmail.com','17dea7e51732eaa28cc487b62454e329aa94cc853caec1b850128774e2bf767d','P9cj0dAyeX2o4xFGMUzL',0,NULL,NULL),(22,1,'Str','Str','example1@example.com','c43634ad72b0f95a8650f0047f89adcef9a3b5fdebcbbb47716fecb178634f3d','-92o2SqGWXdkUes',0,NULL,NULL),(23,2,'WHite','White','haizenberg@example.com','a17304e530836410c9ae3686167492209c0197b12c1702ecacf1e363e4a0376a','nsWZMGqfgrXn2TE',0,NULL,NULL),(24,2,'Ккk','Dostoyevskiy','testEmail@example.com','adbc99e14c20c9a6802aeb8ad4728a4b5b9d40a1cf735d01c0cd8290a5ce738d','H7h5MEN3yNmb5ch',1,NULL,NULL),(25,2,'Leonardo','DiCaprio','leo@example.com','3b30a648e2844f7654f5eb6b709bcc741b75e5c6a0538e563e044287d2b63092','HBMMpoMDSeqAGEA',0,NULL,NULL),(26,2,'User','User','killMePlease@example.com','ac201b4ada383ffc74c9fe5b515230d0053a82d1657410c2cf34c0b434619d40','9MH0EyslOI5vjrx',0,NULL,NULL),(27,2,'Entony','Soprano','tony@example.com','eee61f7d85eb363994617dfa02713d61694e620eb20ced06740ac682849f5fb4','vUAFzDxvdchluq2',0,NULL,NULL),(28,2,'Kate','Middleton','cambridgeroyalty@example.com','fa2b09ed632d0cf703ac979148adbbd491473e4d1935c32394a4f91169341cec','xwgGMRYtWCEOvtk',0,NULL,NULL),(29,2,'Eva','Green','evagreen@gmail.com','3816b4104a68d0c5645b21d164e52b764325b62fb7745d20ae3f01ce7eb327e0','PnGWaDeNCXuYtbZ',0,NULL,NULL),(30,3,'Ser','Artur','secretary@thhhh.com','1ec96f244cccf46150d6b1a8bbbbf25ee6f5136be85d2cb04ebef3043391c462','9rC1d7msDk2GR86',0,NULL,NULL),(31,1,'Maksym','Mozdolevskyi','mexickoe@gmail.com','0536dbcf44e66bfbc5ddcff707dd9499cecbe966595c39fd56b70c4d54e78b16','5FnCcpk2zV4yI9T',0,NULL,NULL),(32,1,'Ruth','Dog','ruru@gmail.com','aef690f6bc263309c52e3b7d83f259fba4519c1f1232eb009790c936866dd428','ndf5Z9jG7tBCk9c',0,NULL,NULL),(33,3,'Max','Mozdolevskyi','kaamos@mail.ru','f66bf2e9fbeaff81ef2760adea58cfa0c2fda205ad343c1433340c008f817765','DSdDRqzclkZw393',0,NULL,NULL),(34,2,'Boris','Бритва','Britva@xn--90aorig.com','cf766ba5a29eac8656a51b07f08a7292f937a17eebb03fb277e3948890b9590c','jOm3xLkVJYpoI9K',1,NULL,NULL),(35,1,'Abudab','Shakhir','abudab@gmail.com','7065af0548136139f74c38f7e2bc3cbd5a0b4c8ce08f0c24a72a0693a21eeb15','RatuWGbP194CELf',1,NULL,NULL),(36,2,'Looooooooooooooooooooooooong','Naaaaaaaaaaaaaaaaame','anas@gmail.com','17014709b0a8570dfcb81acb47a27a4944008f90075702247e0ed9defcc8f608','fCxKnj4Y1cpH8a6',0,NULL,NULL),(37,1,'Anastasia','Gordienko','anast@gmail.com','adee39ef31e1ee3986f395d3f67e0a7fc993c0a1d08ed1ca96d13ceb37752ad9','KvDJmHLQtpRpt4d',1,NULL,NULL),(38,1,'Pavel','Kundenko','pavel228@gmail.com','ffc1135b147482712110241863f9b3fac8e90b812da63d9197f6b6f3fe0656d5','gR0YEqcuxLLRUvb',1,NULL,NULL),(39,3,'Вито','Карлионе','vitocorleone@vito.cor','7387c9481f4be99d101f200f17101ab4eb9a98c282e52f78efab6f0bccf3ad8c','1SvLcJr2h38dNZd',0,NULL,NULL),(40,3,'Vito','Корлеонеп','vitocorleone@cor.vito','04d5b0e3a28b3d5915a92b03a11cbfbe686013f33c6a41eaf04574553ed3a87a','H0qP9zSm2WhhibR',0,NULL,NULL),(41,3,'Andreyо','Roroпппп','andre@gmail.com','a48e18e5995e499b98eca16d809a1dcf301df1a804d339c2198633b485018107','MR2WJFzCS3jU-BU',0,NULL,NULL),(42,2,'Oksana','Stepnova','xxxgvi@gmail.com','c5e6d854a8a0725a89697c93ba88083644dd761d532c611149889649fe8de1dc','X7-t3opZz9Y3lqr',0,NULL,NULL),(43,1,'eq','qwe','qwe@hotmail.com','f5f5a20c9ad989ab4bf51d10db1517d98bf78e9471ea9dfc3c8d6cb2641bbab7','DdTdmHX8A9xiRyp',0,NULL,NULL),(44,1,'qwe','qwe','1234@hot.com','43bd75e044de67410879a18809a6bf770d08f6822b050a3cedcb9079e244b11e','oyZvzXg009Y5RCj',0,NULL,NULL),(45,1,'Nazar','Bodan','wolfter@ukr.net','d25b0f0733cf9c0d61bf583b4f94ea441afb05aca1e7d22a61e2a11f774a54bb','1c9mFjkyIn4nrTJ',1,NULL,NULL),(46,3,'Nazar','Bodanre','wolfter12@gmail.com','f27e1a94d79d0d9a0f5ae82570a9e4d05edfdeee9bfa090c7962245b13d90de0','c6jMu1LNI1fLoFt',0,NULL,NULL),(47,1,'Alex','Tarasenko','alextarasenko@gmail.com','56ae8dc549c3ca14adc10ab0be72c1c1fe6f48eceddee6c792489819a137f364','PcL2mDQBueg4Ovb',1,NULL,NULL),(48,3,'SecretaryGGG','BlahGGcds','sec@gmail.comGGGG','b2d1f16bc4912dc0dad74bb87a820a077bb6d6556ddfb7130c091ce65ca76440','H3aX1rm14sUsmAx',0,NULL,NULL),(49,2,'Mentor','Blah','men@gmail.com','c68539b59ec3bf478f3bd8fadb7eaa44a1783cc620efa37df7d4f35acb01ad96','AQcl9xyGpbGMinP',1,NULL,NULL),(50,3,'secc','secc','S2ss@gmail.com','406006a363b65a5ee83054dfe98cad5b0a666277a7a7a808c7fd843b5855d0be','1c4c-Dq0hbslMSx',0,NULL,NULL),(51,3,'Seccc','Secccв','seccc@gmail.com','380b7f320eaf45c173031ed9ba8a5a86f06421a0b08ab4cdc5391ec7dfc90f4c','JlRRBELHYZwYwm3',1,NULL,NULL),(52,1,'Lana','Rey','lana@gmail.com','c3cf5e6179afad217e0e6d53be3caf1bf2ff2d3de420d61944bf5a83f9d545f6','rJ3h6ubyBo57DqR',0,NULL,NULL),(53,2,'Mentor','Surname','mentor2@gmail.com','b033b811ae37b7afbb6601663da733fc4e7d96a3eabab14714573a8b43cc5551','Y-CEgXFb7lGe8Sm',0,NULL,NULL),(54,2,'Mentorr','LastName','mentor3@gmail.com','d650b6c739fe9a3ccc9284412a49acfb89ff808b7e8f17b49a3495348decd035','j-CFNwLcNHhrMTL',0,NULL,NULL),(55,2,'Вера','Морозова','Vera@gmail.com','169c10caa191f5c104aad543de559e142a28b4ff458c11241eb0032e5855fa6f','hw3MW-G2gQVgtaD',0,NULL,NULL),(56,2,'Oksanaa','Stepnova','oxxi@gmail.com','b497534b412127b90932786c078b2d43334f5080ff78c7992c8fd46878a71f3d','Ns5Dam6qXjTcEGq',0,NULL,NULL),(57,3,'Liliyanna','Konnungen','Lilli@a.b','7b1eb3ba15ce74d2c3ca8215c5aeb90da31e89b6f8a20c46b6afe959cb818c24','NL3fQZDX9lgCtXs',1,NULL,NULL),(58,2,'Mask','Skam','maskksam@gmail.com','a7c8ccd58a5e16fca8187c1bde62b802676dcb3ee12bb5f6024bc20e414915a3','0w6ThssFwzSuphy',0,NULL,NULL),(59,2,'asdasd','asdadsd','gsrg@gmail.com','d77e885a774a1f151d0361db0479bf99b35e52105c8167f70482e33e0b8da0e0','zTDnEDgZyZLfhUb',0,NULL,NULL),(60,2,'Vlad','shkut','VLad@sy.com','bdd7b014aad5ae4584c6cc62da17f467f1cffb7f1dad705a994978db6157b1c2','fOFohFZaWcl5Nx2',0,NULL,NULL),(61,3,'Secretary','Test','secretaryfortest@gmail.com','f4e167245090176adacf4fca7eeafc13986c965ad3835a8ea7af0786f4895ed6','PDh6fmorjD9cIn4',0,NULL,NULL),(62,1,'Student','Test','studentfortest@gmail.com','b492c92da606b7d40bc4103f98d03df71fcddddd0c92c46e157212dd18dbb3d2','NXYfQm1LkATirVa',1,NULL,NULL),(63,2,'Mentor','Test','mentorfortest@gmail.com','f468eb6aaf2eead03d97274921296a6cffe3ed226e035c31619d860d854ac88f','E8AaJIXphBCbc4Q',1,NULL,NULL),(64,3,'Userrrпыыы','Userr','jj@gmail.com','3a0f16a76a3ad073be0391cadf61720cfb4792e94626618fb83a1d0e90888da7','8ZTTzm4VXOpD3z8',0,NULL,NULL),(65,3,'Test','User','test.user@net.negde','3537329c894e4ece5896f85410a92e88941d7cc67abe6f3ded0b2b4e77257d96','dsxVRkonhH2vIux',0,NULL,NULL),(66,3,'Lil','One','meks@gmail.com','954d230aacea0a516d18cf4155854673581c9f085a9ca8c0e07c2ea65fd0c95e','tGxmnt54DlV9aPZ',0,NULL,NULL),(67,2,'mentorone','dontDelete','mentorone@gmail.com','6921ddeb054ed6fcbca360551ef1203958d0ddb771be9a99e538b89e3d9d9a5d','r5Bcz8PiMfLYILB',1,NULL,NULL),(68,2,'mentortwo','dontDelete','mentortwo@gmail.com','0fdcc107b1be3ed72a947f2c9d0193a3ffeec9cb9c2f54fe323a86bd7d6901c9','06U1tsLeoOqvrli',1,NULL,NULL),(69,2,'mentorthree','dontDelete','mentorthree@gmail.com','e03abb672c0f3e1acf813b2f8b95574d57a14cdf8da1307f7574b677078b65fc','MDkIamSUxJqQxGV',1,NULL,NULL),(70,2,'mentorfour','dontDelete','mentorfour@gmail.com','0b779e7299cab302d49452c35fd10708767fe39631d73519a246a2887fc3bb21','YALNsNy2FjcPuvt',1,NULL,NULL),(71,2,'mentorfive','dontDelete','mentorfive@gmail.com','ab17486c72b60a1eca7f65f227192312306a9d9f7ad20085eada2ca81d3f9172','HryTOCJZWb2cPHZ',1,NULL,NULL),(72,2,'mentorsix','dontDelete','mentorsix@gmail.com','2002f238f6659e7ec65deed3d546823263ad59e3790062017918a141943f9679','WA3nfPKGGcwU7HU',1,NULL,NULL),(73,2,'mentorseven','dontDelete','mentorseven@gmail.com','b19d06ce4798a7e6687734f65a5ca6942a2240e32fcf649359e386af2b7a3b8e','aRAvAeXXtY6zq7b',1,NULL,NULL),(74,2,'mentoreight','dontDelete','mentoreight@gmail.com','49ee292def67591092eb9e33f6e9912d429e95397ec9187d607453363c7656bd','aQbZ6l3giQQPWC6',1,NULL,NULL),(75,3,'secretary','secretary','secretary@test.test','cb95ac71535f3280eb5d9804a9c041fb6ad5270f43ee68165481e02d59eb4321','Jx0Y34Ab6L7psd9',1,NULL,NULL);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `attachment`
--

LOCK TABLES `attachment` WRITE;
/*!40000 ALTER TABLE `attachment` DISABLE KEYS */;
INSERT INTO `attachment` VALUES (1,'2021-01-14 12:00:24',1,'9587b11e00f848a391ee5dbcf9baba1d','catpic.jpg'),(2,'2021-01-21 15:25:27',1,'9fe00be2c66b40f883dd9e5cca395153','catpic.jpg'),(3,'2021-01-21 21:38:39',1,'bdce66802b9b4558ac310e1897928f03','catpic.jpg');
/*!40000 ALTER TABLE `attachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `attachment_of_homework`
--

LOCK TABLES `attachment_of_homework` WRITE;
/*!40000 ALTER TABLE `attachment_of_homework` DISABLE KEYS */;
/*!40000 ALTER TABLE `attachment_of_homework` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `course`
--

LOCK TABLES `course` WRITE;
/*!40000 ALTER TABLE `course` DISABLE KEYS */;
INSERT INTO `course` VALUES (1,'Курс для демо',1),(2,'разработка на Java',0),(3,'создание игр на Java',0),(4,'создание веб приложений',1),(5,'курс для разработкчиков системы1',0),(6,'HTML development',1),(7,'fantasy111',0),(8,'UI UX design elementary',0),(9,'Basics of JS1',0),(10,'How to be successful is life',0),(11,'Advanced JS',1),(12,'course сds',0),(13,'Angular',1),(14,'React JS',1),(15,'Java in 1 hour',1),(16,'Яркое солнце',0),(17,'cvbvcb',1),(18,'Programming For Linux',1),(19,'Course about JS1',0),(20,'Hero Course',1),(21,'Java',1),(22,'course for deleting',0),(23,'QC course',0),(24,'SQL Course',1),(25,'Testing Theory',0),(26,'a1',0),(27,'имя курса',1),(28,'1a',0),(29,'Ракета',0),(30,'КУРС ПО СОЗДАНИЮ ДОКУМЕНТАЦИИ',0),(31,'Курс для демо1',1),(32,'1a1',1),(33,'11111111111',1),(34,'122222',1);
/*!40000 ALTER TABLE `course` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `event_occurence`
--

LOCK TABLES `event_occurence` WRITE;
/*!40000 ALTER TABLE `event_occurence` DISABLE KEYS */;
INSERT INTO `event_occurence` VALUES (4,1,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,5),(5,2,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,3),(6,2,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,6),(7,3,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,1),(9,4,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,4),(10,3,'2010-01-10 10:00:00','2020-01-10 11:00:00',2,6),(11,4,'2010-01-10 10:00:00','2020-01-10 11:00:00',1,2),(12,1,'2021-01-21 21:53:08','2021-01-21 21:53:08',1,546),(14,1,'2021-01-21 21:59:42','2021-02-21 22:59:42',1,546),(15,1,'2021-01-21 22:07:02','2021-02-21 22:07:02',1,546),(16,1,'2021-01-21 22:35:47','2021-02-21 23:35:47',1,546),(17,1,'2021-01-21 22:42:54','2021-02-21 22:42:54',1,546),(18,1,'2021-01-21 22:46:10','2021-02-21 22:46:10',1,546),(20,1,'2021-01-21 23:16:50','2021-02-21 23:16:50',1,546),(22,1,'2021-01-21 23:55:26','2021-02-21 23:55:26',1,546),(27,1,'2021-01-22 00:23:50','2021-01-23 00:23:50',1,546),(28,1,'2021-01-22 00:23:50','2021-02-25 00:23:50',1,546),(29,1,'2021-01-22 10:57:16','2021-02-22 10:57:16',1,546),(30,1,'2021-01-22 15:10:05','2021-02-22 15:10:05',1,546);
/*!40000 ALTER TABLE `event_occurence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `homework`
--

LOCK TABLES `homework` WRITE;
/*!40000 ALTER TABLE `homework` DISABLE KEYS */;
/*!40000 ALTER TABLE `homework` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `lesson`
--

LOCK TABLES `lesson` WRITE;
/*!40000 ALTER TABLE `lesson` DISABLE KEYS */;
INSERT INTO `lesson` VALUES (1,2,1,7,'2020-02-01 00:00:00'),(2,2,3,5,'2020-12-24 12:20:00'),(3,3,2,11,'2020-12-26 16:40:00'),(4,3,3,10,'2020-12-25 15:28:00'),(5,8,2,12,'2020-12-23 23:16:00'),(6,8,2,13,'2020-12-17 14:33:00'),(7,5,4,14,'2020-12-16 14:33:00'),(8,4,2,15,'2020-12-16 14:36:00'),(9,4,1,16,'2020-12-28 14:39:00'),(10,8,1,17,'2020-12-02 14:39:00'),(11,4,2,18,'2020-12-23 14:37:00'),(12,7,2,9,'2021-01-12 01:49:00'),(13,4,1,19,'2021-01-03 17:19:00'),(14,2,4,20,'2021-07-20 18:30:25'),(15,2,4,20,'2021-07-20 18:30:25'),(16,10,4,21,'2021-01-07 17:20:00'),(17,4,1,19,'2022-06-17 22:52:00'),(18,25,11,23,'2021-01-29 19:56:22'),(19,26,11,23,'2021-01-24 20:07:33');
/*!40000 ALTER TABLE `lesson` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `mentor`
--

LOCK TABLES `mentor` WRITE;
/*!40000 ALTER TABLE `mentor` DISABLE KEYS */;
INSERT INTO `mentor` VALUES (1,4),(2,5),(3,6),(4,23),(7,24),(8,25),(12,26),(9,27),(6,28),(5,29),(10,34),(11,36),(13,42),(14,49),(16,53),(15,54),(17,55),(18,56),(20,58),(19,59),(21,60),(22,63),(23,67),(24,68),(25,69),(26,70),(27,71),(28,72),(29,73),(30,74);
/*!40000 ALTER TABLE `mentor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `mentor_of_course`
--

LOCK TABLES `mentor_of_course` WRITE;
/*!40000 ALTER TABLE `mentor_of_course` DISABLE KEYS */;
INSERT INTO `mentor_of_course` VALUES (30,1,1),(15,2,6),(17,3,6),(12,3,9),(36,4,2),(2,4,3),(16,5,6),(11,5,9),(27,7,4),(18,7,6),(34,7,18),(31,9,7),(29,16,13);
/*!40000 ALTER TABLE `mentor_of_course` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `mentor_of_student_group`
--

LOCK TABLES `mentor_of_student_group` WRITE;
/*!40000 ALTER TABLE `mentor_of_student_group` DISABLE KEYS */;
INSERT INTO `mentor_of_student_group` VALUES (185,1,7),(186,1,9),(187,1,10),(198,2,3),(168,3,8),(154,4,1),(169,4,8),(171,4,9),(173,4,10),(182,6,2),(87,6,4),(174,6,11),(176,6,12),(178,6,13),(180,6,14),(192,7,2),(191,7,3),(190,7,4),(205,7,19),(211,7,26),(128,9,1),(130,9,6),(175,9,11),(177,9,12),(179,9,13),(181,9,14),(199,10,16),(184,13,4),(188,14,15),(201,14,17),(203,14,18),(196,18,6),(197,18,7),(195,18,12),(189,20,15),(212,22,26),(200,23,16),(202,23,17),(204,23,18),(214,23,28),(209,24,24),(206,25,19),(208,25,21),(207,30,20),(210,30,25),(213,30,26),(215,30,28);
/*!40000 ALTER TABLE `mentor_of_student_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `scheduled_event`
--

LOCK TABLES `scheduled_event` WRITE;
/*!40000 ALTER TABLE `scheduled_event` DISABLE KEYS */;
INSERT INTO `scheduled_event` VALUES (1,22,1,6,5,NULL,'2021-01-25 23:55:25','2021-01-25 23:55:25'),(2,4,3,1,2,1,'2010-01-10 10:00:00','2020-01-10 11:00:00'),(3,4,3,1,2,2,'2010-01-10 10:00:00','2020-01-10 11:00:00'),(4,6,6,1,2,3,'2015-01-10 10:00:00','2020-01-10 11:00:00'),(5,5,4,1,2,4,'2016-01-10 10:00:00','2020-01-10 11:00:00'),(6,7,3,1,2,5,'2018-01-10 10:00:00','2020-01-10 11:00:00'),(7,10,3,1,2,6,'2019-01-10 10:00:00','2020-01-10 11:00:00'),(8,4,3,1,2,7,'2010-01-10 10:00:00','2020-01-10 11:00:00'),(9,11,3,1,2,8,'2020-01-10 10:00:00','2020-01-10 11:00:00'),(10,27,1,6,5,NULL,'2021-01-22 00:23:50','2021-01-22 00:23:50'),(11,28,1,6,5,NULL,'2021-01-25 00:23:50','2021-01-25 00:23:50'),(12,28,1,6,5,NULL,'2021-02-08 00:23:50','2021-02-08 00:23:50'),(13,28,1,6,5,NULL,'2021-02-22 00:23:50','2021-02-22 00:23:50'),(14,28,1,6,5,NULL,'2021-01-22 00:23:50','2021-01-22 00:23:50'),(15,28,1,6,5,NULL,'2021-02-05 00:23:50','2021-02-05 00:23:50'),(16,28,1,6,5,9,'2021-02-19 00:23:50','2021-02-19 00:23:50'),(17,29,1,6,5,NULL,'2021-01-25 10:57:15','2021-01-25 10:57:15'),(18,29,1,6,5,NULL,'2021-02-08 10:57:15','2021-02-08 10:57:15'),(19,29,1,6,5,NULL,'2021-02-22 10:57:15','2021-02-22 10:57:15'),(20,29,1,6,5,NULL,'2021-01-22 10:57:15','2021-01-22 10:57:15'),(21,29,1,6,5,NULL,'2021-02-05 10:57:15','2021-02-05 10:57:15'),(22,29,1,6,5,NULL,'2021-02-19 10:57:15','2021-02-19 10:57:15'),(23,30,1,6,5,NULL,'2021-01-25 15:10:05','2021-01-25 15:10:05'),(24,30,1,6,5,NULL,'2021-02-08 15:10:05','2021-02-08 15:10:05'),(25,30,1,6,5,NULL,'2021-02-22 15:10:05','2021-02-22 15:10:05'),(26,30,1,6,5,NULL,'2021-01-22 15:10:05','2021-01-22 15:10:05'),(27,30,1,6,5,NULL,'2021-02-05 15:10:05','2021-02-05 15:10:05'),(28,30,1,6,5,NULL,'2021-02-19 15:10:05','2021-02-19 15:10:05');
/*!40000 ALTER TABLE `scheduled_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `secretary`
--

LOCK TABLES `secretary` WRITE;
/*!40000 ALTER TABLE `secretary` DISABLE KEYS */;
INSERT INTO `secretary` VALUES (1,2),(2,3),(4,30),(5,33),(6,39),(7,40),(8,41),(9,46),(10,48),(11,50),(12,51),(13,57),(14,61),(15,64),(16,65),(17,66),(18,75);
/*!40000 ALTER TABLE `secretary` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (1,7),(2,8),(3,9),(4,10),(5,11),(6,12),(7,13),(8,14),(9,15),(10,16),(11,17),(12,18),(13,19),(14,20),(15,21),(17,22),(16,31),(20,32),(18,35),(19,37),(21,38),(23,43),(22,44),(24,45),(25,47),(26,52),(27,62);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `student_group`
--

LOCK TABLES `student_group` WRITE;
/*!40000 ALTER TABLE `student_group` DISABLE KEYS */;
INSERT INTO `student_group` VALUES (1,4,'Griffindorrr','2020-09-01','2022-09-01'),(2,1,'Slytherin','2019-09-01','2022-09-01'),(3,11,'Ravenclaww','2018-09-01','2021-09-01'),(4,6,'Hufflepuff','2018-09-01','2021-09-01'),(6,4,'changeName','0001-01-01','0001-01-01'),(7,17,'SX-14','2015-07-20','2015-09-20'),(8,17,'SX-15','2015-07-20','2015-09-20'),(9,2,'SX-16','2015-07-20','2015-09-20'),(10,2,'SX-17','2015-07-20','2015-09-20'),(11,17,'SX-3','2015-07-20','2015-09-20'),(12,17,'SX-5','2015-07-20','2015-09-20'),(13,17,'SX-8','2015-07-20','2015-09-20'),(14,17,'SX-21','2015-07-20','2015-09-20'),(15,4,'KM-71','2021-01-23','2021-02-06'),(16,12,'KM-79','2021-01-22','2021-02-07'),(17,5,'KP-93','2021-01-05','2021-02-07'),(18,13,'KP-94','2021-01-05','2021-02-07'),(19,10,'KM-92','2021-01-21','2021-02-05'),(20,9,'KP-90','2021-01-28','2021-03-11'),(21,14,'KM-80','2021-01-17','2021-04-11'),(22,2,'groupik','2021-01-08','2021-01-08'),(23,5,'groupikk','2021-01-07','2021-01-31'),(24,8,'KВ-34','2021-01-06','2021-02-07'),(25,14,'KP-91','2021-01-23','2021-02-07'),(26,14,'КП-89','2021-01-09','2021-02-07'),(27,2,'groupikkk','2021-01-14','2021-01-17'),(28,6,'group2','2021-01-08','2021-01-31');
/*!40000 ALTER TABLE `student_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `student_of_student_group`
--

LOCK TABLES `student_of_student_group` WRITE;
/*!40000 ALTER TABLE `student_of_student_group` DISABLE KEYS */;
INSERT INTO `student_of_student_group` VALUES (296,4,15),(297,4,8),(298,4,11),(299,4,14),(300,4,17),(302,NULL,NULL),(407,1,12),(408,1,6),(409,1,10),(410,1,21),(411,1,15),(412,1,14),(413,1,13),(414,1,11),(415,1,3),(416,6,3),(418,6,6),(419,6,12),(420,1,3),(421,6,3),(422,1,3),(423,1,3),(424,6,3),(430,4,17),(432,1,17),(433,1,8),(440,3,8),(441,3,14),(442,3,10),(443,3,15),(452,10,2),(453,10,3),(455,11,10),(456,11,11),(458,12,10),(459,12,11),(461,13,10),(462,13,11),(464,14,10),(465,14,11),(466,2,12),(467,2,13),(468,2,14),(469,4,1),(470,9,1),(471,10,1),(472,15,11),(473,15,12),(474,15,15),(475,12,5),(476,11,5),(477,14,5),(478,3,5),(479,13,5),(480,1,5),(481,16,21),(482,16,27),(483,17,12),(484,17,27),(485,18,12),(486,18,27),(487,19,25),(488,19,27),(489,20,19),(490,21,24),(491,24,27),(492,25,11),(493,26,5),(494,26,19),(495,26,27);
/*!40000 ALTER TABLE `student_of_student_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `theme`
--

LOCK TABLES `theme` WRITE;
/*!40000 ALTER TABLE `theme` DISABLE KEYS */;
INSERT INTO `theme` VALUES (12,'Classes'),(13,'Closure'),(17,'Easdasas'),(15,'Functions'),(23,'Html'),(8,'Inheritance'),(11,'Inheritance Edit'),(19,'Magic'),(21,'Ms Dos'),(9,'New Lesson'),(10,'New Lesson Edit'),(18,'Objects'),(14,'Scope'),(20,'Some theme Ado.Net'),(16,'Themee'),(7,'Types'),(5,'Variables'),(6,'Variables Edited'),(22,'Магия'),(2,'ООП в Java'),(4,'разработка API'),(1,'Создание макета'),(3,'создание простых скриптов на Unity');
/*!40000 ALTER TABLE `theme` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `visit`
--

LOCK TABLES `visit` WRITE;
/*!40000 ALTER TABLE `visit` DISABLE KEYS */;
INSERT INTO `visit` VALUES (52,5,4,12,1,''),(53,10,4,5,1,''),(54,14,4,0,1,''),(55,15,4,0,1,''),(56,9,3,4,1,''),(57,10,3,6,1,''),(58,12,3,0,1,''),(59,13,3,0,1,''),(60,5,3,0,1,''),(61,3,3,NULL,0,''),(62,6,3,NULL,0,''),(63,1,3,NULL,0,''),(87,15,7,11,1,''),(88,8,7,0,1,''),(89,11,7,10,1,''),(90,14,7,0,0,''),(91,17,7,0,0,''),(92,1,7,0,0,''),(93,14,8,12,1,''),(94,13,8,10,1,''),(95,12,8,0,1,''),(96,8,8,0,0,''),(97,17,8,0,0,''),(98,1,8,0,0,''),(121,14,11,8,1,''),(122,13,11,9,1,''),(123,12,11,10,1,''),(124,8,11,0,0,''),(125,17,11,0,0,''),(126,1,11,0,0,''),(170,14,5,7,1,''),(171,13,5,12,1,''),(172,12,5,10,1,''),(173,8,5,0,0,''),(174,17,5,0,0,''),(175,1,5,0,0,''),(176,14,6,10,1,''),(177,13,6,4,1,''),(178,12,6,0,0,''),(179,8,6,12,1,''),(180,17,6,5,1,''),(181,1,6,0,0,''),(229,3,9,0,0,''),(230,13,9,0,0,''),(231,14,9,0,0,''),(232,15,9,0,0,''),(233,21,9,0,0,''),(234,10,9,0,0,''),(235,6,9,8,1,''),(236,12,9,8,1,''),(237,11,9,12,1,''),(238,5,9,5,1,''),(239,1,14,5,1,''),(240,14,14,NULL,0,''),(241,11,14,NULL,0,''),(242,8,14,NULL,0,''),(243,15,14,NULL,0,''),(244,1,15,5,1,''),(245,14,15,NULL,0,''),(246,11,15,NULL,0,''),(247,8,15,NULL,0,''),(248,10,15,NULL,0,''),(279,14,12,0,1,''),(280,13,12,12,1,''),(281,12,12,12,1,''),(282,8,12,0,0,''),(283,17,12,0,0,''),(304,15,16,10,1,''),(305,8,16,0,0,''),(306,11,16,0,1,''),(307,14,16,5,1,''),(308,17,16,0,1,''),(329,5,2,0,0,''),(330,10,2,5,1,''),(331,14,2,4,1,''),(332,15,2,5,1,''),(333,8,2,5,1,''),(334,17,1,11,1,''),(335,11,1,8,1,''),(336,13,1,3,1,''),(337,14,1,3,1,''),(338,15,1,4,1,''),(339,21,1,3,1,''),(340,10,1,5,1,''),(341,12,1,4,1,''),(342,8,1,0,0,''),(343,5,1,12,1,''),(354,17,13,0,0,''),(355,11,13,0,0,''),(356,13,13,0,0,''),(357,14,13,0,0,''),(358,15,13,0,0,''),(359,21,13,0,0,''),(360,10,13,NULL,0,''),(361,12,13,NULL,0,''),(362,8,13,NULL,0,''),(363,5,13,0,0,''),(364,14,10,0,0,''),(365,11,10,0,0,''),(366,8,10,0,0,''),(367,15,10,0,0,''),(368,12,10,0,0,''),(369,13,10,0,0,''),(370,5,10,0,0,''),(371,17,10,0,0,''),(372,10,10,0,0,''),(373,21,10,0,0,''),(384,8,17,0,0,''),(385,11,17,0,0,''),(386,13,17,0,0,''),(387,14,17,0,0,''),(388,15,17,0,0,''),(389,21,17,0,0,''),(390,10,17,0,0,''),(391,12,17,0,0,''),(392,17,17,0,0,''),(393,5,17,0,0,''),(394,10,18,2,1,''),(395,11,18,10,1,''),(396,5,18,0,0,''),(397,10,19,4,1,''),(398,11,19,0,0,''),(399,5,19,0,0,'');
/*!40000 ALTER TABLE `visit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'soft'
--

--
-- Dumping routines for database 'soft'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-01-26 14:27:55
