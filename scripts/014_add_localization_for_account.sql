USE soft;
ALTER TABLE accounts
  ADD Localization varchar(10) DEFAULT 'en-US'
	after AvatarID
