DO $body$
DECLARE
    u_admin uuid := gen_random_uuid();
    u_mgr uuid := gen_random_uuid();
    
    cat_power uuid := gen_random_uuid();
    cat_hand uuid := gen_random_uuid();
    cat_fast uuid := gen_random_uuid();
    cat_plumb uuid := gen_random_uuid();
    cat_elec uuid := gen_random_uuid();
    
    sup_makita uuid := gen_random_uuid();
    sup_dewalt uuid := gen_random_uuid();
    sup_milw uuid := gen_random_uuid();
    sup_bosch uuid := gen_random_uuid();
    sup_gen uuid := gen_random_uuid();
    
    wh_main uuid := gen_random_uuid();
    wh_sec uuid := gen_random_uuid();
    wh_west uuid := gen_random_uuid();
    
    p_mak_drill uuid := gen_random_uuid();
    p_mak_saw uuid := gen_random_uuid();
    p_dew_drill uuid := gen_random_uuid();
    p_dew_grind uuid := gen_random_uuid();
    p_mil_impact uuid := gen_random_uuid();
    p_mil_light uuid := gen_random_uuid();
    p_bos_hammer uuid := gen_random_uuid();
    p_bos_laser uuid := gen_random_uuid();
    p_ham_claw uuid := gen_random_uuid();
    p_wrench_set uuid := gen_random_uuid();
    p_screw_flat uuid := gen_random_uuid();
    p_screw_phil uuid := gen_random_uuid();
    p_nail_galv uuid := gen_random_uuid();
    p_pipe_pvc uuid := gen_random_uuid();
    p_wire_12g uuid := gen_random_uuid();
    
    po1 uuid := gen_random_uuid();
    po2 uuid := gen_random_uuid();
    
    so1 uuid := gen_random_uuid();
    so2 uuid := gen_random_uuid();
    so3 uuid := gen_random_uuid();
BEGIN
    -- Clear existing data safely
    TRUNCATE TABLE "InventoryTransactions" CASCADE;
    TRUNCATE TABLE "SalesOrderItems" CASCADE;
    TRUNCATE TABLE "SalesOrders" CASCADE;
    TRUNCATE TABLE "PurchaseOrderItems" CASCADE;
    TRUNCATE TABLE "PurchaseOrders" CASCADE;
    TRUNCATE TABLE "Stocks" CASCADE;
    TRUNCATE TABLE "Products" CASCADE;
    TRUNCATE TABLE "Categories" CASCADE;
    TRUNCATE TABLE "Suppliers" CASCADE;
    TRUNCATE TABLE "Warehouses" CASCADE;
    TRUNCATE TABLE "Users" CASCADE;

    -- USERS
    INSERT INTO "Users" ("Id", "FirstName", "LastName", "Email", "PasswordHash", "Role", "IsActive", "CreatedAt") VALUES 
    (u_admin, 'Admin', 'User', 'admin@stocksphere.com', 'admin123', 0, true, NOW()),
    (u_mgr, 'Manager', 'User', 'manager@stocksphere.com', 'manager123', 1, true, NOW());

    -- CATEGORIES
    INSERT INTO "Categories" ("Id", "Name", "Description", "CreatedAt") VALUES 
    (cat_power, 'Power Tools', 'Electric and battery-operated tools', NOW()),
    (cat_hand, 'Hand Tools', 'Manual tools for basic operations', NOW()),
    (cat_fast, 'Fasteners', 'Screws, nails, bolts, nuts', NOW()),
    (cat_plumb, 'Plumbing', 'Pipes, fittings, and plumbing accessories', NOW()),
    (cat_elec, 'Electrical', 'Wires, outlets, switches', NOW());

    -- SUPPLIERS
    INSERT INTO "Suppliers" ("Id", "Name", "ContactPerson", "Email", "Phone", "Address", "IsActive", "CreatedAt") VALUES 
    (sup_makita, 'Makita Corp', 'John Doe', 'sales@makita.com', '123-456-7890', '123 Makita Way', true, NOW()),
    (sup_dewalt, 'DeWalt Tools', 'Jane Smith', 'orders@dewalt.com', '800-433-9258', '701 E Joppa Rd', true, NOW()),
    (sup_milw, 'Milwaukee Tool', 'Mike Tool', 'dist@milwaukee.com', '800-729-3878', '13135 W Lisbon Rd', true, NOW()),
    (sup_bosch, 'Bosch Power Tools', 'Robert Bosch', 'sales@boschtools.com', '877-267-2499', '1800 W Central Rd', true, NOW()),
    (sup_gen, 'Generic Supplies Inc', 'Tom Tom', 'sales@generichw.com', '555-555-5555', '555 Generic Blvd', true, NOW());

    -- WAREHOUSES
    INSERT INTO "Warehouses" ("Id", "Name", "Location", "Capacity", "IsActive", "CreatedAt") VALUES 
    (wh_main, 'Main NY Depot', 'New York, NY', 5000, true, NOW()),
    (wh_sec, 'Secondary NJ Hub', 'Newark, NJ', 2000, true, NOW()),
    (wh_west, 'West Coast Storage', 'Los Angeles, CA', 3000, true, NOW());

    -- PRODUCTS
    INSERT INTO "Products" ("Id", "Sku", "Name", "Description", "PurchasePrice", "SellingPrice", "ImageUrl", "ReorderLevel", "IsActive", "CreatedAt", "CategoryId", "SupplierId", "Manufacturer", "PartNumber", "WeightKg", "Dimensions", "UnitOfMeasure") VALUES 
    (p_mak_drill, 'MAK-18V-DRILL', 'Makita 18V Cordless Drill', 'Brushless cordless drill kit', 120.00, 199.99, '', 10, true, NOW(), cat_power, sup_makita, 'Makita', 'XFD131', 1.5, '20x15x10', 'Each'),
    (p_mak_saw, 'MAK-18V-SAW', 'Makita 18V Circular Saw', 'Cordless circular saw 6-1/2"', 140.00, 229.99, '', 5, true, NOW(), cat_power, sup_makita, 'Makita', 'XSS02Z', 3.3, '35x25x20', 'Each'),
    (p_dew_drill, 'DEW-20V-DRILL', 'DeWalt 20V Max Drill', 'Compact drill driver kit', 115.00, 189.99, '', 10, true, NOW(), cat_power, sup_dewalt, 'DeWalt', 'DCD771C2', 1.6, '22x16x11', 'Each'),
    (p_dew_grind, 'DEW-20V-GRIND', 'DeWalt 20V Angle Grinder', '4-1/2 inch angle grinder', 130.00, 179.99, '', 8, true, NOW(), cat_power, sup_dewalt, 'DeWalt', 'DCG412B', 1.8, '30x12x12', 'Each'),
    (p_mil_impact, 'MIL-M18-IMPACT', 'Milwaukee M18 Impact', 'Hex impact driver', 135.00, 199.99, '', 15, true, NOW(), cat_power, sup_milw, 'Milwaukee', '2850-20', 1.4, '18x14x9', 'Each'),
    (p_mil_light, 'MIL-M18-LIGHT', 'Milwaukee M18 Work Light', 'LED work light', 45.00, 79.99, '', 20, true, NOW(), cat_power, sup_milw, 'Milwaukee', '2735-20', 0.8, '10x10x20', 'Each'),
    (p_bos_hammer, 'BOS-18V-HAMMER', 'Bosch 18V Hammer Drill', 'Bulldog rotary hammer', 180.00, 279.99, '', 5, true, NOW(), cat_power, sup_bosch, 'Bosch', 'GBH18V-26D', 2.6, '40x20x15', 'Each'),
    (p_bos_laser, 'BOS-LASER-LVL', 'Bosch Cross-Line Laser', 'Self-leveling laser level', 110.00, 159.99, '', 10, true, NOW(), cat_power, sup_bosch, 'Bosch', 'GLL55', 0.5, '15x15x15', 'Each'),
    (p_ham_claw, 'GEN-CLAW-HAM', 'Stanley 16oz Claw Hammer', 'Fiberglass handle claw hammer', 8.50, 14.99, '', 30, true, NOW(), cat_hand, sup_gen, 'Stanley', 'STHT51304', 0.7, '33x14x3', 'Each'),
    (p_wrench_set, 'GEN-WRENCH-SET', 'Craftsman 11pc Wrench Set', 'Metric combination wrench set', 25.00, 44.99, '', 15, true, NOW(), cat_hand, sup_gen, 'Craftsman', 'CMMT12053', 1.2, '30x10x5', 'Set'),
    (p_screw_flat, 'GEN-SCREW-FLAT', 'Flathead Screwdriver', 'Standard 6-inch flathead', 3.00, 6.99, '', 50, true, NOW(), cat_hand, sup_gen, 'Klein', '600-6', 0.1, '25x3x3', 'Each'),
    (p_screw_phil, 'GEN-SCREW-PHIL', 'Phillips Screwdriver', '#2 Phillips 4-inch', 3.00, 6.99, '', 50, true, NOW(), cat_hand, sup_gen, 'Klein', '603-4', 0.1, '20x3x3', 'Each'),
    (p_nail_galv, 'FAS-NAIL-GALV', 'Galvanized Nails 2"', '1lb box of 2-inch galvanized nails', 4.50, 8.99, '', 100, true, NOW(), cat_fast, sup_gen, 'Grip-Rite', '2HG1', 0.45, '10x10x10', 'Box'),
    (p_pipe_pvc, 'PLU-PVC-2IN', '2" PVC Pipe', 'Schedule 40 PVC pipe 10ft', 12.00, 18.99, '', 50, true, NOW(), cat_plumb, sup_gen, 'Charlotte Pipe', 'PVC040200600', 3.0, '300x5x5', 'Piece'),
    (p_wire_12g, 'ELE-WIRE-12G', '12/2 Romex Wire', '250ft roll of indoor electrical wire', 75.00, 125.99, '', 10, true, NOW(), cat_elec, sup_gen, 'Southwire', '28828269', 11.5, '30x30x15', 'Roll');

    -- STOCKS
    INSERT INTO "Stocks" ("Id", "ProductId", "WarehouseId", "Quantity", "LastUpdated", "Aisle", "Rack", "Bin") VALUES 
    (gen_random_uuid(), p_mak_drill, wh_main, 45, NOW(), 'A1', 'Rack 1', 'Bin 12'),
    (gen_random_uuid(), p_mak_drill, wh_west, 15, NOW(), 'W4', 'Rack 2', 'Bin 5'),
    (gen_random_uuid(), p_mak_saw, wh_main, 8, NOW(), 'A1', 'Rack 1', 'Bin 15'), -- LOW STOCK
    (gen_random_uuid(), p_dew_drill, wh_main, 50, NOW(), 'A2', 'Rack 1', 'Bin 1'),
    (gen_random_uuid(), p_dew_grind, wh_sec, 22, NOW(), 'S1', 'Rack 3', 'Bin 9'),
    (gen_random_uuid(), p_mil_impact, wh_main, 35, NOW(), 'A3', 'Rack 1', 'Bin 4'),
    (gen_random_uuid(), p_mil_light, wh_west, 60, NOW(), 'W2', 'Rack 5', 'Bin 20'),
    (gen_random_uuid(), p_bos_hammer, wh_main, 6, NOW(), 'A4', 'Rack 1', 'Bin 1'),
    (gen_random_uuid(), p_bos_laser, wh_sec, 12, NOW(), 'S2', 'Rack 2', 'Bin 2'),
    (gen_random_uuid(), p_ham_claw, wh_main, 120, NOW(), 'B1', 'Rack 10', 'Bin 50'),
    (gen_random_uuid(), p_wrench_set, wh_main, 45, NOW(), 'B2', 'Rack 11', 'Bin 22'),
    (gen_random_uuid(), p_screw_flat, wh_main, 200, NOW(), 'B3', 'Rack 12', 'Bin 1'),
    (gen_random_uuid(), p_screw_phil, wh_main, 180, NOW(), 'B3', 'Rack 12', 'Bin 2'),
    (gen_random_uuid(), p_nail_galv, wh_sec, 300, NOW(), 'C1', 'Rack 20', 'Bin 100'),
    (gen_random_uuid(), p_pipe_pvc, wh_west, 150, NOW(), 'P1', 'Rack 1', 'Floor'),
    (gen_random_uuid(), p_wire_12g, wh_main, 40, NOW(), 'E1', 'Rack 5', 'Pallet A');

    -- TRANSACTIONS (To populate Analytics dashboard)
    INSERT INTO "InventoryTransactions" ("Id", "ProductId", "WarehouseId", "Type", "Quantity", "ReferenceNumber", "Remarks", "TransactionDate") VALUES 
    (gen_random_uuid(), p_mak_drill, wh_main, 0, 50, 'PO-1001', 'Initial Stock', NOW() - INTERVAL '10 days'),
    (gen_random_uuid(), p_mak_drill, wh_main, 1, 5, 'SO-2001', 'Sale to Home Depot', NOW() - INTERVAL '5 days'),
    (gen_random_uuid(), p_mak_saw, wh_main, 0, 10, 'PO-1002', 'Initial Stock', NOW() - INTERVAL '15 days'),
    (gen_random_uuid(), p_mak_saw, wh_main, 1, 2, 'SO-2002', 'Retail Sale', NOW() - INTERVAL '2 days'),
    (gen_random_uuid(), p_dew_drill, wh_main, 0, 50, 'PO-1003', 'Restock', NOW() - INTERVAL '8 days'),
    (gen_random_uuid(), p_pipe_pvc, wh_west, 0, 200, 'PO-1004', 'Bulk purchase', NOW() - INTERVAL '20 days'),
    (gen_random_uuid(), p_pipe_pvc, wh_west, 1, 50, 'SO-2003', 'Contractor Sale', NOW() - INTERVAL '1 days');

END $body$;
