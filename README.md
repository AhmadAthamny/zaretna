# Overview - What's this?
My farmer uncle that has been farming for more than 15 years (works as agriculture engineer as well) decided to start selling fruits & vegetables online, delivered directly from the ground to the customer's house, so my elder brother made him a website, and I was there to help in making the "back-office" windows application that manages the website and the business, such as managing orders, products, customers, delivery, statistics and more.

# Managing Products
![image](https://user-images.githubusercontent.com/36423427/234229345-ebac3f5a-0e6c-4217-a9a4-f9dda7a280f0.png)
* Managing products is user-friendly.
* Same product can appear on multiple categories.
* Categories can be added/modified through the same window.
* Specific products order can be made for each category, user friendly editor as well.

# Managing Orders
![image](https://user-images.githubusercontent.com/36423427/234231926-f55b2959-24ef-4e91-abe0-628a29939edc.png)

This is the window to view and manage orders.
* Orders with golden background are collected, weighted (priced), and are ready for delivery.
* Orders with white background are not collected yet.
* Supports advanced search (filter by delivery region, customer's first name/last name, phone number, show orders between two dates).
* To view an order, we simply click on it, and the following window will open:
![image](https://user-images.githubusercontent.com/36423427/234238179-d1d429a5-fbbd-4b4d-bcb4-d8a4e501595c.png)
When complete, we write the final price of the order (after weighting the products and getting the exact price), then the order will have the golden background.

# Delivery
This is the delivery man window:
![image](https://user-images.githubusercontent.com/36423427/234234666-f5dd12a4-9932-4ef4-8376-b0a0a908fa62.png)

* When an order is collected and a final price was set for it, then the order will appear in the delivery window.
* We can then send an email to the delivery man that contains a table of collected orders with all needed details.
* Here's an example of an email sent from the program for delivery:
![image](https://user-images.githubusercontent.com/36423427/234241462-02f6f5c8-b27e-4947-832c-ea3a1c8f4649.png)
* When an order is delivered, by clicking on the link on the most left column, the order is set as "delivered" and will disappear from the orders list.



All project files were moved to this Github repository for the first time.

The website is hosted on bluehost.com, the host was supposed to be renewed in the last March (03/2023) in order to keep the website up, but it's currently "under maintenence".

Required packages:
1. MySQL.Data (By Oracle)
