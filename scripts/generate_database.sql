ALTER TABLE soft.account
ADD COLUMN forgot_token_gen_date DATETIME NULL DEFAULT NULL COMMENT 'date of generation for users forgot password token';