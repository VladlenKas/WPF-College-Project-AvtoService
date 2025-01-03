-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: avtoservice_3curs_aa
-- ------------------------------------------------------
-- Server version	8.0.39

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
-- Table structure for table `sale`
--

DROP TABLE IF EXISTS `sale`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sale` (
  `id_sale` int NOT NULL AUTO_INCREMENT,
  `id_employee` int NOT NULL,
  `id_carclient` int NOT NULL,
  `id_typeofrepair` int NOT NULL,
  `date` datetime(5) NOT NULL,
  `cost_for_client` int NOT NULL,
  `cost_total` int NOT NULL,
  PRIMARY KEY (`id_sale`),
  KEY `id_employee_idx` (`id_employee`),
  KEY `id_typeofrepair_idx` (`id_typeofrepair`),
  KEY `id_client_idx` (`id_carclient`),
  CONSTRAINT `id_carclient` FOREIGN KEY (`id_carclient`) REFERENCES `carclient` (`id_carclient`),
  CONSTRAINT `id_employee` FOREIGN KEY (`id_employee`) REFERENCES `employee` (`id_employee`),
  CONSTRAINT `id_typeofrepair` FOREIGN KEY (`id_typeofrepair`) REFERENCES `typeofrepair` (`id_typeofrepair`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sale`
--

LOCK TABLES `sale` WRITE;
/*!40000 ALTER TABLE `sale` DISABLE KEYS */;
INSERT INTO `sale` VALUES (12,4,53,1,'2024-12-04 02:16:24.36546',14400,18000),(13,4,53,2,'2024-12-04 02:16:41.17941',2000,2000),(14,4,48,1,'2024-12-04 09:18:41.20424',123200,154000),(15,4,59,1,'2024-12-06 11:06:10.26768',18007,22509),(16,4,59,2,'2024-12-06 11:07:30.99567',60500,60500),(17,4,48,2,'2024-12-07 11:10:07.59463',20509,20509),(18,4,48,2,'2024-12-08 17:30:14.37178',22500,22500),(19,4,47,1,'2024-12-11 05:12:16.76853',3600,4500),(20,4,59,1,'2024-12-11 05:53:38.68815',2000,2500),(21,4,48,1,'2024-12-11 05:54:29.86316',10800,13500),(22,4,48,2,'2024-12-11 05:54:47.23060',13500,13500),(23,4,59,1,'2024-12-11 05:56:59.50722',4800,6000),(24,4,47,2,'2024-12-11 09:32:06.88199',2000,2000),(25,4,47,2,'2024-12-11 09:33:34.50024',2000,2000),(26,4,47,1,'2024-12-19 16:40:40.44870',5200,6500),(27,4,47,1,'2024-12-23 11:05:42.60734',4400,5500),(28,4,59,2,'2024-12-27 06:21:44.73170',4000,4000),(29,4,47,1,'2024-12-27 06:24:11.97667',52000,65000);
/*!40000 ALTER TABLE `sale` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-29  5:51:27
