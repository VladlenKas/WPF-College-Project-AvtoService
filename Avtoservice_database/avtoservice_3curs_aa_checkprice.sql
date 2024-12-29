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
-- Table structure for table `checkprice`
--

DROP TABLE IF EXISTS `checkprice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `checkprice` (
  `id_checkprice` int NOT NULL AUTO_INCREMENT,
  `id_sale` int NOT NULL,
  `id_price` int NOT NULL,
  PRIMARY KEY (`id_checkprice`),
  KEY `id_price_idx` (`id_price`),
  KEY `id_sale_idx` (`id_sale`),
  CONSTRAINT `id_price` FOREIGN KEY (`id_price`) REFERENCES `price` (`id_price`),
  CONSTRAINT `id_sale` FOREIGN KEY (`id_sale`) REFERENCES `sale` (`id_sale`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `checkprice`
--

LOCK TABLES `checkprice` WRITE;
/*!40000 ALTER TABLE `checkprice` DISABLE KEYS */;
INSERT INTO `checkprice` VALUES (15,12,6),(16,13,4),(17,14,4),(18,15,1),(19,15,4),(20,15,5),(21,15,6),(22,15,12),(23,15,13),(24,15,15),(25,15,16),(26,15,19),(27,17,1),(28,17,5),(29,17,13),(30,17,12),(31,17,19),(32,17,15),(33,17,6),(34,17,4),(35,18,1),(36,18,5),(37,18,4),(38,18,6),(39,18,12),(40,18,13),(41,18,15),(42,18,16),(43,19,1),(44,19,4),(45,20,5),(46,21,6),(47,21,12),(48,21,13),(49,21,15),(50,22,6),(51,22,12),(52,22,13),(53,22,15),(54,23,6),(55,24,4),(56,25,4),(57,26,5),(58,26,4),(59,26,16),(60,27,4),(61,27,12),(62,28,4),(63,28,16);
/*!40000 ALTER TABLE `checkprice` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-29  5:51:28
