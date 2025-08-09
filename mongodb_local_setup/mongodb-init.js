// MongoDB Initialization Script for AspireLearning
// This script creates the database, collections, and inserts sample data

// Switch to AspireLearning database (creates it if it doesn't exist)
use('AspireLearning');

// Drop existing collections if they exist (for clean setup)
db.catalogs.drop();
db.orders.drop();

// Create catalogs collection and insert sample data
db.catalogs.insertMany([
  {
    "_id": "1692eed5-f3fe-4c94-a767-e7b9bfb41552",
    "Name": "The Clean Code",
    "description": "A Handbook of Agile Software Craftsmanship by Robert C. Martin",
    "catalogType": "Book",
    "price": NumberDecimal("45.99"),
    "availableStock": NumberInt("25"),
    "availableForSell": NumberInt("23")
  },
  {
    "_id": "2a3b4c5d-6e7f-8g9h-0i1j-2k3l4m5n6o7p",
    "Name": "Design Patterns",
    "description": "Elements of Reusable Object-Oriented Software by Gang of Four",
    "catalogType": "Book",
    "price": NumberDecimal("52.99"),
    "availableStock": NumberInt("15"),
    "availableForSell": NumberInt("15")
  },
  {
    "_id": "3b4c5d6e-7f8g-9h0i-1j2k-3l4m5n6o7p8q",
    "Name": "Clean Architecture",
    "description": "A Craftsman's Guide to Software Structure and Design by Robert C. Martin",
    "catalogType": "Book",
    "price": NumberDecimal("48.99"),
    "availableStock": NumberInt("30"),
    "availableForSell": NumberInt("28")
  },
  {
    "_id": "4c5d6e7f-8g9h-0i1j-2k3l-4m5n6o7p8q9r",
    "Name": "MongoDB Atlas Course",
    "description": "Complete MongoDB Atlas cloud database course",
    "catalogType": "Course",
    "price": NumberDecimal("129.99"),
    "availableStock": NumberInt("100"),
    "availableForSell": NumberInt("100")
  },
  {
    "_id": "5d6e7f8g-9h0i-1j2k-3l4m-5n6o7p8q9r0s",
    "Name": "ASP.NET Core in Action",
    "description": "Building modern web applications with ASP.NET Core",
    "catalogType": "Book",
    "price": NumberDecimal("55.99"),
    "availableStock": NumberInt("20"),
    "availableForSell": NumberInt("18")
  },
  {
    "_id": "6e7f8g9h-0i1j-2k3l-4m5n-6o7p8q9r0s1t",
    "Name": "Docker Deep Dive",
    "description": "A comprehensive guide to containerization with Docker",
    "catalogType": "Book",
    "price": NumberDecimal("42.99"),
    "availableStock": NumberInt("12"),
    "availableForSell": NumberInt("12")
  },
  {
    "_id": "7f8g9h0i-1j2k-3l4m-5n6o-7p8q9r0s1t2u",
    "Name": "Microservices Patterns",
    "description": "Designing microservices architecture patterns",
    "catalogType": "Book",
    "price": NumberDecimal("59.99"),
    "availableStock": NumberInt("8"),
    "availableForSell": NumberInt("7")
  },
  {
    "_id": "8g9h0i1j-2k3l-4m5n-6o7p-8q9r0s1t2u3v",
    "Name": "Cloud Computing Fundamentals",
    "description": "Introduction to cloud computing concepts and services",
    "catalogType": "Course",
    "price": NumberDecimal("89.99"),
    "availableStock": NumberInt("50"),
    "availableForSell": NumberInt("50")
  }
]);

// Create orders collection and insert sample data
db.orders.insertMany([
  {
    "_id": "cd8d1f2f-307c-4e52-b055-44c5b361fab7",
    "OrderDate": new Date("2024-01-15T10:30:00Z"),
    "Delivered": true,
    "OrderItems": [
      {
        "CatalogItemId": "1692eed5-f3fe-4c94-a767-e7b9bfb41552",
        "OrderedQuantity": NumberInt("2"),
        "SuppliedQuantity": NumberInt("2")
      },
      {
        "CatalogItemId": "2a3b4c5d-6e7f-8g9h-0i1j-2k3l4m5n6o7p",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("1")
      }
    ]
  },
  {
    "_id": "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6",
    "OrderDate": new Date("2024-01-16T14:20:00Z"),
    "Delivered": false,
    "OrderItems": [
      {
        "CatalogItemId": "3b4c5d6e-7f8g-9h0i-1j2k-3l4m5n6o7p8q",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("0")
      },
      {
        "CatalogItemId": "4c5d6e7f-8g9h-0i1j-2k3l-4m5n6o7p8q9r",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("0")
      }
    ]
  },
  {
    "_id": "b2c3d4e5-f6g7-8h9i-0j1k-l2m3n4o5p6q7",
    "OrderDate": new Date("2024-01-17T09:15:00Z"),
    "Delivered": true,
    "OrderItems": [
      {
        "CatalogItemId": "5d6e7f8g-9h0i-1j2k-3l4m-5n6o7p8q9r0s",
        "OrderedQuantity": NumberInt("3"),
        "SuppliedQuantity": NumberInt("3")
      }
    ]
  },
  {
    "_id": "c3d4e5f6-g7h8-9i0j-1k2l-m3n4o5p6q7r8",
    "OrderDate": new Date("2024-01-18T16:45:00Z"),
    "Delivered": false,
    "OrderItems": [
      {
        "CatalogItemId": "6e7f8g9h-0i1j-2k3l-4m5n-6o7p8q9r0s1t",
        "OrderedQuantity": NumberInt("2"),
        "SuppliedQuantity": NumberInt("1")
      },
      {
        "CatalogItemId": "7f8g9h0i-1j2k-3l4m-5n6o-7p8q9r0s1t2u",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("0")
      }
    ]
  },
  {
    "_id": "d4e5f6g7-h8i9-0j1k-2l3m-n4o5p6q7r8s9",
    "OrderDate": new Date("2024-01-19T11:30:00Z"),
    "Delivered": true,
    "OrderItems": [
      {
        "CatalogItemId": "8g9h0i1j-2k3l-4m5n-6o7p-8q9r0s1t2u3v",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("1")
      },
      {
        "CatalogItemId": "1692eed5-f3fe-4c94-a767-e7b9bfb41552",
        "OrderedQuantity": NumberInt("1"),
        "SuppliedQuantity": NumberInt("1")
      }
    ]
  },
  {
    "_id": "e5f6g7h8-i9j0-1k2l-3m4n-o5p6q7r8s9t0",
    "OrderDate": new Date("2024-01-20T08:00:00Z"),
    "Delivered": false,
    "OrderItems": [
      {
        "CatalogItemId": "2a3b4c5d-6e7f-8g9h-0i1j-2k3l4m5n6o7p",
        "OrderedQuantity": NumberInt("2"),
        "SuppliedQuantity": NumberInt("0")
      }
    ]
  }
]);

// Create indexes for better performance
db.catalogs.createIndex({ "Name": 1 });
db.catalogs.createIndex({ "catalogType": 1 });
db.catalogs.createIndex({ "price": 1 });

db.orders.createIndex({ "OrderDate": -1 });
db.orders.createIndex({ "Delivered": 1 });
db.orders.createIndex({ "OrderItems.CatalogItemId": 1 });

print("✅ Database 'AspireLearning' created successfully!");
print("✅ Collections 'catalogs' and 'orders' created successfully!");
print("✅ Sample data inserted successfully!");
print("📊 Catalog items count: " + db.catalogs.countDocuments());
print("📊 Orders count: " + db.orders.countDocuments());
print("");
print("🎉 MongoDB setup complete! You can now run your AspireLearning application.");
print("🔍 Use MongoDB Compass to connect to: mongodb://localhost:27017"); 