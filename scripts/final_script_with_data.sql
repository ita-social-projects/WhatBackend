USE `Soft`;

LOCK TABLES `Accounts` WRITE;

INSERT INTO `Accounts`
(`Role`, `FirstName`, `LastName`, `Email`, `PasswordHash`, `Salt`)
VALUES
(4,'James', 'Smith', 'james.smith@example.com', 'b9583289d283a24df5560cc3a5cd736a11db2391d8b75d910a529e8eb012fd76', 'RIERl-Dl8qSM5zETI4e3aGYx6v8AtIgi'),
(8,'Robert', 'Jones', 'robert.jones@example.com', 'ee93e6d106bb48bdd5e149069c6a3f914b47c7da597456dcf6073709a30eaf95', 'UWllVbE-F9FUa0nYeKe6nyGz2PJ3s93w'),
(8,'John', 'Williams', 'john.williams@example.com', 'e50cc224bf395f5fb549ad03a99bec90c5313486f321a53325cf2e5a06196598', 'WqMNJ38X9h-WQrXVfok7PIfPcRrjxkO7'),
(2,'Michael', 'Taylor', 'michael.taylor@example.com', '26af26655782b750999cc0034833d443028a2b719cbf74bad3a4336cea5c3313', '6nyPtWV9Xv28nbPfQIHoaEHf1xqBYlIk'),
(10,'William', 'Davies', 'william.davies@example.com', '0547f6a778c455b7da3bbe86e1193aa5f4313c7a9f5177898e70d1f86698f554', 'YM4sD28ufPQlsL3jdJXERcXIXLZOaJ4n'),
(3,'David', 'Brown', 'david.brown@example.com', 'd69c222bd3ed192e43d5d4be0b1e6d6d0b4eab182ce854c73d82e587c36ad20a', 'aCUOXekU58GiAGeI48M0Gj6XYuJtyqTb'),
(3,'Richard', 'Thomas', 'richard.thomas@example.com', 'ea7c52e3e4244fe19a12eecfebc0ff23ee0e6ab26a04a28e5ef7cc8ef49c42e5', 'gZNU3fYetVVIWnwg9VDddhFIq64GJvDQ'),
(3,'Joseph', 'Evans', 'joseph.evans@example.com', '3a9236462c58e0d01c746cc47d7a4d44a4123acbf548cc08d1eb5a63006e786f', 'wrf58BpD8510wNTiJdDWVXy13M130Wyw'),
(1,'Thomas', 'Roberts', 'thomas.roberts@example.com', 'b605a5da487a3643f394ea261dda0c290b3a40fbdb8d33b49f3a62fe8c3f42f8', 'YvuwDLH0jqEFh9ocE4VY5zF97RvVBwGd'),
(1,'Charles', 'Johnson', 'charles.johnson@example.com', 'a72acc8432900fb918d49d19f015bcc27853f66e57835ff8fb2ea9d79b8fe72d', 'wruhr0bL9qWewLPPsra8BH64gNX2v91C'),
(1,'Christopher', 'Wilson', 'christopher.wilson@example.com', '7ede226950673cfa937dd1670b5b1b1c8e6d7d603e8d2d0ef7e920891b29935b', 'V6j6Muam4gq-NksfIRU1VHAK3jMSZi8h'),
(1,'Daniel', 'Robinson', 'daniel.robinson@example.com', 'e786a55b37bc9b8a29426a300c190dbc091267d702332971fa12038414b9e9fc', 'hAsOkK5eUZrw2rL1pjhsyKCxG1tB-xFM'),
(1,'Matthew', 'Wright', 'matthew.wright@example.com', 'de0e81769468fcf0324a91a4252b5517151dbb6d5e8b5676c5c112fe787ce376', 'eOLFbIKc30wOl2CqLiD-kmEgihAMEtDn'),
(1,'Anthony', 'Wood', 'anthony.wood@example.com', 'b9f82f5e4cd00c2d011270db3bc4ad479bf0a42a53d38642cb88ca1cbcd169bd', '0HcjYIT84vS7-qxNmUo8BgM8KVhHILDH'),
(1,'Mark', 'Thompson', 'mark.thompson@example.com', '7586c48ff4a660912456106453e031a4c0cc5c9ce88380fe7f9752e092e68157', 'ZBErx85aJLUDn3GHycIkf556D9Ot0oQ3'),
(1,'Donald', 'Hall', 'donald.hall@example.com', 'c80612525a80169807878c65d58047df365d425b5c02da5949feda725b7fceb3', 'ehIWVosE7t2QOHtnOGn9uIBK6u4tkrVe'),
(1,'Steven', 'Green', 'steven.green@example.com', 'a4171502814d47be649f35ba7a82d28979f1706c39ca121a1794a90a89012542', 'FZXZcstL2vbwua8im1UDTKFBO3iHXUxz'),
(1,'Paul', 'Walker', 'paul.walker@example.com', '5386882a23a2b727fbfae17d757226d1f1e543aeba7973bfec7336302cf295e4', 'KxtxqSZ10Zac8w6PQwDC-gycmPgW-hnD'),
(1,'Andrew', 'Hughes', 'andrew.hughes@example.com', 'e9c30108e9a2b14b5329525fa46068d3a5c7e29a2f9c398cf2100c149a8fdb44', '73Emp3ARiSev7vzZ5C4QVgmbRhKWCtB8'),
(1,'Joshua', 'Edwards', 'joshua.edwards@example.com', 'f38f24e1b82b964dda21d9ebb7c3d9ebb6e93ff2f3c7448af9ad106fa6393564', 'J5-dp09bXOmeJBUf8BcRUDsBxUjpA490'),
(4,'Mary', 'Lewis', 'mary.lewis@example.com', '9d824589f4f1e8bf614e0986769703f1d381db1673b96fc57cd37533c8f38431', 'VXE5AQ8TVkIlJVxIEQKINW9LaYwsQcAr'),
(8,'Patricia', 'White', 'patricia.white@example.com', '3306c36d3668d16d1a495bdf66967be5881da2e57b981a47eb0cef45ad817f84', 't14l00w-m-FMRGkwjHOPv4HQKDzRiuk0'),
(8,'Jennifer', 'Turner', 'jennifer.turner@example.com', 'ed2967867b397f04880f6de42dca41e36365b3f0a5101454faa9da996035c544', 'Ur4FRZR2usYfLDc346UAFKJg2xNmmhv4'),
(2,'Linda', 'Jackson', 'linda.jackson@example.com', 'c47b39275236a29a84f912c69a0b43116a96da0975e0870447b5383557064ed9', 'BR3mbMtQ-wO4ogcocUBcTxa-5KzXgnED'),
(10,'Elizabeth', 'Hill', 'elizabeth.hill@example.com', 'c19e3178e1c2b423cd0ded134d6ce1eef8722f32ff3adb32114128123761f0ef', '4bnxlbP34ZGIRYUTqPVWIdZwsF795IfR'),
(3,'Barbara', 'Harris', 'barbara.harris@example.com', 'cdb28cfbd020652f05266d7f3a4f5a6683e4b08f041c43790180d9385a81c7f1', 'qzIuXmZ6tVZa16Pzd90h70P229owjWX5'),
(3,'Susan', 'Clark', 'susan.clark@example.com', '80b39d102e6280f2e1060718422152561ef7bfad2224ffbc4e036be6dcfd3eec', 'PU5NdILgXyTVUOsymzdgaGhBUzfjLOOK'),
(3,'Jessica', 'Cooper', 'jessica.cooper@example.com', '7ae277e7f16fad6879be897a2ce6381577a6d9a9fa0da68cd89754992126f54f', 'jiFqK8ntl1yd3yLSy7SbH6L7EvfcPfdy'),
(1,'Sarah', 'Harrison', 'sarah.harrison@example.com', '46d1899a962ebd1f7daba8f54a52c95434117de8aa793801bce9f4f274e7ede2', 'yG0REmGL5pqJ5dMRH8mfr2p6Cyr2wrth'),
(1,'Karen', 'Ward', 'karen.ward@example.com', 'b3c311d7d43f22fb651298eddf4c035281d946b1d214f565484f484b7e6f480c', 'M97w9giAQe8y48OrzXrWUijb-UFx5UgM'),
(1,'Nancy', 'Martin', 'nancy.martin@example.com', '75154eb1f8bcc48bd6567c85fd6cde521ae67247f54cd986ad38ab52b112841a', 'AizxVvrTd4tl7tAOz9VeS3kZoQEuSpr5'),
(1,'Lisa', 'Davis', 'lisa.davis@example.com', 'd1acc190d0d43c130cfdecd8e4c1b5fa72f3bb1dfdd6fbf692ca857d5b2ce2f0', 'WvOgEkOqh2zDzstVks21Vd2Wf4XBckAb'),
(1,'Betty', 'Baker', 'betty.baker@example.com', '48435a4b61a50d40ef082f6627d7ba6714fda229153be708457e2c091334baff', 'EVH5Xjfr4r7yn4jLMHiC4bmgUz9ARVjU'),
(1,'Margaret', 'Morris', 'margaret.morris@example.com', '3c67d5fce00c3a7c45b9d256cfb934a31271b751b2f2786f870ed849c28962e4', 'GCMGGIWLIZbPy9314bVmTeWOCdrai5DS'),
(1,'Sandra', 'James', 'sandra.james@example.com', 'd4bb6e45e93f6a83b054b11bb35d414b0cd80b216be0bf0c258cf9f4febe67e4', 'ki11bC60FwN2FKWHtta0ynLhRtRy8Qpc'),
(1,'Ashley', 'King', 'ashley.king@example.com', 'b98b91414c33d523d0f419a54ef7ad532b5c8be3e7407d572888ff6db8385b3b', 'mIRlQAzE75VEDhDsZnlgjAQ2882NkCv2'),
(1,'Kimberly', 'Morgan', 'kimberly.morgan@example.com', '107eea3a0bb06173dee9eaf1f063b6e879232ad469e160f97342fb5082ea3d69', 'WT5W8qEh9XIqNBabVQIBrxnWj4kVBhUW'),
(1,'Emily', 'Allen', 'emily.allen@example.com', 'e7fd151eae718b0fa25922471babd7a28367e34f1551e6c288aec5796ea85542', 'pxyKClZuEFCkQHctCApQwx6EIOpYJM3J'),
(1,'Donna', 'Moore', 'donna.moore@example.com', '27d06279e0cb742ba5004ff8642eff7f2711778f00598f4c10b3e4d0d849b794', 'jOjBzqKvxiVJAL65xi04QulK8FQMDLH3'),
(1,'Michelle', 'Parker', 'michelle.parker@example.com', '63bbd827350f66d49e1fa3b8f43226f32637a0d9f07cdbc7afa3df0131bb117d', 'Yz0YA413tQOafiI9DEmG0GSor8r2EvvD')
;

UNLOCK TABLES;

LOCK TABLES `Mentors` WRITE;

INSERT INTO `Mentors`
(`AccountID`)
VALUES
(4),
(5),
(6),
(7),
(8),
(24),
(25),
(26),
(27),
(28)
;

UNLOCK TABLES;

LOCK TABLES `Students` WRITE;

INSERT INTO `Students`
(`AccountID`)
VALUES
(6),
(7),
(8),
(9),
(10),
(11),
(12),
(13),
(14),
(15),
(16),
(17),
(18),
(19),
(20),
(26),
(27),
(28),
(29),
(30),
(31),
(32),
(33),
(34),
(35),
(36),
(37),
(38),
(39),
(40)
;

UNLOCK TABLES;

LOCK TABLES `Secretaries` WRITE;

INSERT INTO `Secretaries`
(`AccountID`)
VALUES
(2),
(3),
(5),
(22),
(23),
(25)
;

UNLOCK TABLES;

LOCK TABLES `Courses` WRITE;

INSERT INTO `Courses`
(`Name`)
VALUES
('Soft Skills for Lecturers'),
('C# Programming'),
('3D Modelling'),
('Cybersecurity'),
('Advanced Technical English'),
('Intermediate Technical English')
;

UNLOCK TABLES;

LOCK TABLES `MentorsOfCourses` WRITE;

INSERT INTO `MentorsOfCourses`
(`MentorID`, `CourseID`)
VALUES
(1, 1),
(6, 1),
(3, 2),
(8, 2),
(4, 3),
(9, 3),
(5, 4),
(10, 4),
(2, 5),
(7, 5),
(2, 6),
(7, 6)
;

UNLOCK TABLES;

LOCK TABLES `StudentGroups` WRITE;

INSERT INTO `StudentGroups`
(`CourseID`, `Name`, `StartDate`, `FinishDate`)
VALUES
(1, 'Soft Skills for Lecturers - 2021/2', '2021-07-05', '2021-12-24'), -- 2 times per month
(2, 'C# Programming - 2021/2', '2021-07-05', '2021-12-24'), -- 2 times per week
(3, '3D Modelling - 2021/2', '2021-07-05', '2021-12-24'), -- 2 times per week
(4, 'Cybersecurity - 2021/2', '2021-07-05', '2021-12-24'), -- 2 times per week
(5, 'Advanced Technical English - 2021', '2021-01-04', '2021-12-24'),  -- 2 times per week
(6, 'Intermediate Technical English - 2021', '2021-01-04', '2021-12-24')  -- 2 times per week
;

UNLOCK TABLES;

LOCK TABLES `StudentsOfStudentGroups` WRITE;

INSERT INTO `StudentsOfStudentGroups`
(`StudentGroupID`, `StudentID`)
VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 16),
(1, 17),
(1, 18),
(2, 4),
(2, 5),
(2, 6),
(2, 7),
(2, 19),
(2, 20),
(2, 21),
(2, 22),
(3, 8),
(3, 9),
(3, 10),
(3, 11),
(3, 23),
(3, 24),
(3, 25),
(3, 26),
(4, 12),
(4, 13),
(4, 14),
(4, 15),
(4, 27),
(4, 28),
(4, 29),
(4, 30),
(5, 4),
(5, 5),
(5, 6),
(5, 7),
(5, 8),
(5, 9),
(5, 19),
(5, 20),
(5, 21),
(5, 22),
(5, 23),
(5, 24),
(6, 10),
(6, 11),
(6, 12),
(6, 13),
(6, 14),
(6, 15),
(6, 25),
(6, 26),
(6, 27),
(6, 28),
(6, 29),
(6, 30)
;

UNLOCK TABLES;

LOCK TABLES `MentorsOfStudentGroups` WRITE;

INSERT INTO `MentorsOfStudentGroups`
(`MentorID`, `StudentGroupID`)
VALUES
(1, 1),
(6, 1),
(3, 2),
(8, 2),
(4, 3),
(9, 3),
(5, 4),
(10, 4),
(2, 5),
(7, 5),
(2, 6),
(7, 6)
;

UNLOCK TABLES;

LOCK TABLES `Themes` WRITE;

INSERT INTO `Themes`
(`Name`)
VALUES
('Introduction'),
('Leadership'),
('Communication'),
('Teamwork'),
('Problem Solving'),
('Social and Emotional Intelligence'),
('Final'),
('Data Types'),
('Exceptions'),
('Collections'),
('Patterns'),
('Sorting Algorithms'),
('Debugging'),
('OOP'),
('LINQ'),
('Entity Framework'),
('Multithreading'),
('Modelling'),
('Shading'),
('Texturing'),
('Lighting'),
('Rendering'),
('Rigging'),
('Animation'),
('Sculpting'),
('3D Printing'),
('Game Asset Creation'),
('Control Hijacking Attacks'),
('Buffer Overflow Exploits and Defenses'),
('Privilege Separation'),
('Sandboxing Native Code'),
('Web Security Model'),
('Symbolic Execution'),
('SSL and HTTPS'),
('Side-Channel Attacks'),
('User Authentication'),
('Data Tracking'),
('Lecture'),
('Discussion')
;

UNLOCK TABLES;

-- LOCK TABLES `EventOccurrences` WRITE;

-- INSERT INTO `EventOccurrences`
-- (`StudentGroupID`, `EventStart`, `EventFinish`, `Pattern`, `Storage`)
-- VALUES
-- (1, '2021-07-05', '2021-12-24', 3)
-- ;

-- UNLOCK TABLES;