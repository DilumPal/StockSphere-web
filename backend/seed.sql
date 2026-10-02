DO $body$
DECLARE
    cat_id uuid := gen_random_uuid();
    sup_id uuid := gen_random_uuid();
    wh_id uuid := gen_random_uuid();
    prod1_id uuid := gen_random_uuid();
    prod2_id uuid := gen_random_uuid();
BEGIN
    INSERT INTO "Categories" ("Id", "Name", "Description", "CreatedAt") 
    VALUES (cat_id, 'Power Tools', 'Electric and battery-operated tools', NOW());

    INSERT INTO "Suppliers" ("Id", "Name", "ContactPerson", "Email", "Phone", "Address", "IsActive", "CreatedAt") 
    VALUES (sup_id, 'Makita Corp', 'John Doe', 'sales@makita.com', '123-456-7890', '123 Makita Way', true, NOW());

    INSERT INTO "Warehouses" ("Id", "Name", "Location", "Capacity", "IsActive", "CreatedAt") 
    VALUES (wh_id, 'Main Hardware Depot', 'Aisle 1-10', 1000, true, NOW());

    INSERT INTO "Products" ("Id", "Sku", "Name", "Description", "PurchasePrice", "SellingPrice", "ImageUrl", "ReorderLevel", "IsActive", "CreatedAt", "CategoryId", "SupplierId", "Manufacturer", "PartNumber", "WeightKg", "Dimensions", "UnitOfMeasure")
    VALUES 
    (prod1_id, 'MAK-18V-DRILL', 'Makita 18V Cordless Drill', 'Brushless cordless drill kit', 120.00, 199.99, '', 5, true, NOW(), cat_id, sup_id, 'Makita', 'XFD131', 1.5, '20x15x10', 'Each'),
    (prod2_id, 'MAK-18V-SAW', 'Makita 18V Circular Saw', 'Cordless circular saw 6-1/2"', 140.00, 229.99, '', 3, true, NOW(), cat_id, sup_id, 'Makita', 'XSS02Z', 3.3, '35x25x20', 'Each');

    INSERT INTO "Stocks" ("Id", "ProductId", "WarehouseId", "Quantity", "LastUpdated", "Aisle", "Rack", "Bin")
    VALUES 
    (gen_random_uuid(), prod1_id, wh_id, 25, NOW(), 'PowerTools', 'Rack 1', 'Bin 12'),
    (gen_random_uuid(), prod2_id, wh_id, 10, NOW(), 'PowerTools', 'Rack 2', 'Bin 5');
END $body$;
