# Pre-requisite
- You have local MongoDB Service configured. Refer to [mongodb-setup.md](mongodb_local_setup/mongodb-setup.md)
- If you prefer to use [free MongoDB cluster](https://www.mongodb.com/cloud/atlas/register?utm_source=google&utm_campaign=search_gs_pl_evergreen_atlas_core-high-int_prosp-brand_gic-null_ww-tier4_ps-all_desktop_eng_lead&utm_term=atlas%20mongodb&utm_medium=cpc_paid_search&utm_ad=p&utm_ad_campaign_id=22031347578&adgroup=173739098633&cq_cmp=22031347578&gad_source=1&gad_campaignid=22031347578&gbraid=0AAAAADQ1403kzPOO6bPF2YQ0jzuhSc7zx&gclid=Cj0KCQjwtMHEBhC-ARIsABua5iTo1BirJE8jZU54QhR6DBU2NAB7uDvcbhQfxWWwm5A3Sib1W-QrW1QaAn_8EALw_wcB), ensure that
  - Network access has been configured and able to connect from local machine
  - Database name in this project is `AspireLearning` with two collections:
    - `catalogs`
    - `orders`
  - Refer to [mongodb-setup.md](mongodb_local_setup\mongodb-setup.md) for injesting test data to your cluster
- You have created event hub namespace
  - `order-delivered` event hub is created with the following three consumer groups and manage access connection string
    - `invoice`
    - `replenishment`
    - `vendor`
- You have created service bus namespace
  - `orders` queue is created with manage connection string

- You have Docker installed
- You have Visual Studio 2022 installed (I am using this to run all projects)
- You have VS Code installed (I am using this to run Vite + React mobile app)


# How to run
1. Search and replace `<SERVICEBUS_CONNSTR>` with your `orders` service bus queue manage connection string
2. Search and replace `<EVENTHUB_CONNSTR>` with your `order-delivered` event hub manage connection string
3. Search and replace `mongodb://localhost:27017` with your MongoDB connection string if you have your free mongodb Atlas cluster
4. Configure Visual Studio 2022 Profile to [start multiple projects](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022):
   - CatalogApi
   - OrderApi
   - WebPortal
   - InvoiceConsumer
   - VendorResupplyConsumer
   - ReplenishmentConsumer
   - OrderProcessor
5. Run Visual Studio 2022 multiple project profile. This will start all project above.
6. Once all projects have run, open up AspireLearning folder using VS Code
   - Open up terminal and change directory to `src\mobileapp`
   - Run `npm install`
   - Run `npm run dev`
   - Open browser and go to http://localhost:5173/ (default vite app port)
   - Set order to deliver