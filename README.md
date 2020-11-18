Some software I wrote a long time ago to help me keep track of invoices I send out to customers.

The software can do the following:

- Convert an invoice to an excel document (which then allows the user to print the invoice off or save it as a pdf to send to a customer)
- Generate yearly reports to see how much you have earnt over the year.
- Keep track of any mileage you have used in yearly reports.
- Search the invoice history in a number of different way.

# To run the program

This software was written a long time ago and I hard coded the location to the database file where all the invoice data is kept.

In order to run the software you will have to edit the part of the code that tells the software where to look for the database file.

The line to edit is in **/Database/connection.cs** on line 15. You need to edit this line to point to the location of where the Invoice.sdf file will be on your computer.

*A better approach would have been to store the database file in 'my documents' but I didn't know any better back then.*



