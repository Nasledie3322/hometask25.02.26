# hometask25.02.26

## Role Based Authorization

The MVC project now includes three built-in roles:

* **SuperAdmin** – full CRUD access across the application and the only account
  that can manage other users and their role assignments.
* **Admin** – may perform create, read, update and delete operations on
  products but cannot modify user roles.
* **User** – read-only access; can view products but cannot change data.

A default SuperAdmin account (`superadmin@localhost` / `SuperAdmin123!`)
is created automatically at startup when the database is empty.  Additional
accounts can be seeded or added via the UI.

### Usage notes

* Registration page (`/Auth/Register`) is available to anonymous visitors –
  new users are automatically assigned the `User` role. After registering
  the account is signed in automatically and you are taken to the products
  list. (If you manually visit a protected page later, unauthenticated
  visitors are sent to `/Auth/Login`; attempting to hit the default
  `/Account/Login` will still 404.)

* If you try to open a page that requires a role you don’t have – for
  example, `/Products/AddProduct` as a normal `User` – you’ll now see an
  **Access Denied** screen instead of being bounced back to login/registration.
* Only SuperAdmin users see the user management menu; they can add users and
  assign them to any role, as well as change existing users' roles.
* Product pages are scoped by role; only Admin and SuperAdmin may add/edit/
  delete.

This README section will help developers or testers understand the role
functionality and default credentials, and is updated as part of the tasks.
