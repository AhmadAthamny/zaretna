# Overview
My farmer uncle that has been farming for more than 15 years (works as agriculture engineer as well) decided to start selling fruits & vegetables online (we called this website Zaretna), delivered directly from the farm to the customer's house, so my elder brother made him a website, and I was there to help in making the "back-office" windows application that manages the website and the business, such as managing orders, products, customers, delivery, statistics and more.

Social media links of Zaretna business:
* Zaretna Facebook Page: https://www.facebook.com/zaretna.co.il
* Zaretna Instagram Page: https://www.instagram.com/zaretna.co.il

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
After collecting and weighting the products, we write the final price of the order (after weighting the products and getting the exact price), then the order will have the golden background.

# Delivery
* When an order is collected and a final price was set for it, then the order will appear in the delivery window.
* Delivery window displays all collected orders, and in it we choose an email address.
* We can then send an email to the delivery man that contains a table of collected orders with all needed details.
* Here's an example of an email sent from the program for delivery:
![image](https://user-images.githubusercontent.com/36423427/234241462-02f6f5c8-b27e-4947-832c-ea3a1c8f4649.png)
* When an order is delivered, by clicking on the link on the most left column, the order is set as "delivered" and will disappear from the orders list in the app, this feature makes the app and the business in general more organized and focused.

# Sales & Statistics 
![image](https://user-images.githubusercontent.com/36423427/234243622-d049efbb-860f-40db-8c5c-8dc04b4791c2.png)
* This is real-time graph, that shows the number of orders between two given dates.
* We can also see more detailed graph, such that it shows how many orders were done from each region between two given dates:
![image](https://user-images.githubusercontent.com/36423427/234244593-2801745d-8a07-44a5-b2a2-972134b11531.png)

# Installation
* Clone this repo.
* Make sure MySQL.Data (By Oracle) is installed.
* Open the cloned project with Visual Studio (2019+).
* Compile the project and run it.
* That's it.
