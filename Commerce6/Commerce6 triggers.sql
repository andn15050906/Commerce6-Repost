--Product: Stock, Sold
CREATE TRIGGER onOrdering ON Order_Products FOR INSERT, UPDATE, DELETE
AS
	BEGIN
		DECLARE @orderedProductId VARCHAR(45), @orderedQuantity INT;
		DECLARE @revertedProductId VARCHAR(45), @revertedQuantity INT;
		DECLARE @remain INT;
		
		SELECT @orderedProductId = ProductId, @orderedQuantity = Quantity FROM inserted;
		SELECT @revertedProductId = ProductId, @revertedQuantity = Quantity FROM deleted;
		
		IF (@orderedQuantity > 0)
			UPDATE Products SET Stock = Stock - @orderedQuantity, Sold = Sold + @orderedQuantity,
				@remain = Stock - @orderedQuantity WHERE Id = @orderedProductId;
		IF (@revertedQuantity > 0)
			UPDATE Products SET Stock = Stock + @revertedQuantity, Sold = Sold - @revertedQuantity WHERE Id = @revertedProductId;

		IF (@remain < 0)
			BEGIN
				RAISERROR(N'Invalid quantity', 16, 1);
				ROLLBACK;
			END
	END
GO

--Shop: FollowerCount
CREATE TRIGGER onFollow ON Follows FOR INSERT, DELETE
AS
	BEGIN
		IF EXISTS (SELECT * FROM INSERTED)
			--Handle INSERT
			UPDATE Shops SET FollowerCount += 1 WHERE Id = (SELECT ShopId FROM inserted)
		ELSE
			--Handle DELETE
			UPDATE Shops SET FollowerCount -= 1 WHERE Id = (SELECT ShopId FROM deleted)
	END
GO

--Product: RatingCount, TotalRating
CREATE TRIGGER onProductRating ON ProductReviews FOR INSERT, UPDATE, DELETE
AS
	BEGIN
		DECLARE @reviewedProductId VARCHAR(45), @reviewedRating INT;
		DECLARE @revertedProductId VARCHAR(45), @revertedRating INT;

		SELECT @reviewedProductId = ProductId, @reviewedRating = Rating FROM inserted;
		SELECT @revertedProductId = ProductId, @revertedRating = Rating FROM deleted;
		
		IF (@reviewedRating IS NULL)
			BEGIN
				SET @reviewedProductId = @revertedProductId;
				SET @reviewedRating = 0;
			END
		IF (@revertedRating IS NULL)
			SET @revertedRating = 0;

		UPDATE Products
		SET RatingCount = RatingCount + (SELECT COUNT(1) FROM inserted) - (SELECT COUNT(1) FROM deleted),
			TotalRating = TotalRating + @reviewedRating - @revertedRating
		WHERE Id = @reviewedProductId;
	END
GO

--Shop: RatingCount, TotalRating
CREATE TRIGGER onShopRating ON ShopReviews FOR INSERT, UPDATE, DELETE
AS
	BEGIN
		DECLARE @reviewedShopId VARCHAR(45), @reviewedRating INT;
		DECLARE @revertedShopId VARCHAR(45), @revertedRating INT;

		SELECT @reviewedShopId = ShopId, @reviewedRating = Rating FROM inserted;
		SELECT @revertedShopId = ShopId, @revertedRating = Rating FROM deleted;
		
		IF (@reviewedRating IS NULL)
			BEGIN
				SET @reviewedShopId = @revertedShopId;
				SET @reviewedRating = 0;
			END
		IF (@revertedRating IS NULL)
			SET @revertedRating = 0;

		UPDATE Shops
		SET RatingCount = RatingCount + (SELECT COUNT(1) FROM inserted) - (SELECT COUNT(1) FROM deleted),
			TotalRating = TotalRating + @reviewedRating - @revertedRating
		WHERE Id = @reviewedShopId;
	END
GO

--Shop: ProductCount
CREATE TRIGGER onProductChange ON Products FOR INSERT, DELETE
AS
	BEGIN
		IF EXISTS (SELECT * FROM INSERTED)
			--Handle INSERT
			UPDATE Shops SET ProductCount += 1 WHERE Id = (SELECT ShopId FROM inserted)
		ELSE
			--Handle DELETE
			UPDATE Shops SET ProductCount -= 1 WHERE Id = (SELECT ShopId FROM deleted)
	END
GO

CREATE TRIGGER orderStatisticsTrigger ON Orders FOR INSERT, DELETE
AS
	BEGIN
		DECLARE @count INT;
		DECLARE @today DATE = CAST(GETDATE() AS DATE);

		IF (SELECT(1) FROM inserted) IS NOT NULL
			BEGIN
				SELECT @count = [Count] FROM OrderStatistics WHERE Date = @today;
				IF (@count > 0)
					UPDATE OrderStatistics SET [Count] = @count + 1 WHERE Date = @today;
				ELSE
					INSERT INTO OrderStatistics VALUES (@today, 1);
			END
		ELSE
			--...
			UPDATE OrderStatistics SET [Count] = [Count] - 1 WHERE Date = @today AND [Count] > 0;
	END