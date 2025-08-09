# MongoDB Local Setup for AspireLearning

## Prerequisites

1. **Install MongoDB Community Edition**
   - Download from: https://www.mongodb.com/try/download/community?tck=docs_server
   - Follow installation instructions for Windows
   - Ensure MongoDB service is running

2. **Install Mongo Shell**
   - Download from: https://www.mongodb.com/try/download/shell?jmp=docs - `MSI` package will auto add `mongosh` to Windows Path
   - If you download `ZIP` package, make sure to add `mongosh` to Windows Path through advanced settings.
   - To verify that `mongosh` successfully added to Windows Path, open command prompt and run `mongosh --version`
   - To verify that `MongoDB` service is running locally, open command prompt and run `mongosh`

3. **Install MongoDB Compass (Optional but Recommended)**
   - Download from: https://www.mongodb.com/try/download/compass
   - Useful for visual database management

## Setup Instructions

### 1. Start MongoDB Service

Ensure MongoDB is running on your local machine:
```bash
# Check if MongoDB service is running
net start MongoDB

# Or start it manually if needed
mongod --dbpath "C:\data\db"
```

### 2. Installation Script to Prepare Local MongoDB Service

```bash
# Ensure that you have change directory to `mongodb_local_setup`
# and run the following on BASH
mongosh < mongodb-init.js
```

### 3. Verify Setup

1. Open MongoDB Compass and connect to `mongodb://localhost:27017`
2. Verify that the `AspireLearning` database exists
3. Check that both `catalogs` and `orders` collections are present
4. Verify sample data is loaded in the `catalogs` collection

## Sample Data

The setup includes sample catalog items and orders for testing your application.

## Connection String

For local development, use:
```
mongodb://localhost:27017
```

## Troubleshooting

1. **MongoDB not starting**: Check Windows Services and ensure MongoDB service is running
2. **Connection issues**: Verify firewall settings and that MongoDB is listening on port 27017
3. **Permission errors**: Run command prompt as administrator when installing or starting services