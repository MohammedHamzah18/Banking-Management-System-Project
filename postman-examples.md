# Postman API Examples

Base URL: `https://localhost:5001`

1. Register
`POST /api/auth/register`
```json
{
  "fullName": "System Admin",
  "email": "admin@bankpro.com",
  "password": "Admin@123",
  "role": "Admin"
}
```

2. Login
`POST /api/auth/login`
```json
{
  "email": "admin@bankpro.com",
  "password": "Admin@123"
}
```

Copy the returned token and set Postman Authorization type to `Bearer Token`.

3. Create Customer
`POST /api/customers`
```json
{
  "fullName": "Nisha Rao",
  "email": "nisha@example.com",
  "phoneNumber": "9000011111",
  "address": "Bengaluru",
  "dateOfBirth": "1995-03-10"
}
```

4. Create Account
`POST /api/accounts`
```json
{
  "customerId": 1,
  "accountType": 1,
  "openingBalance": 25000
}
```

5. Deposit
`POST /api/transactions/deposit`
```json
{
  "accountId": 1,
  "amount": 5000,
  "remarks": "Cash deposit"
}
```

6. Withdraw
`POST /api/transactions/withdraw`
```json
{
  "accountId": 1,
  "amount": 1000,
  "remarks": "ATM withdrawal"
}
```

7. Transfer
`POST /api/transactions/transfer`
```json
{
  "fromAccountId": 1,
  "toAccountId": 2,
  "amount": 2500,
  "remarks": "Rent payment"
}
```

8. Freeze Account
`PATCH /api/accounts/1/status?activate=false`

9. Apply Loan
`POST /api/loans`
```json
{
  "customerId": 1,
  "loanType": "Home Loan",
  "principalAmount": 1200000,
  "annualInterestRate": 8.5,
  "tenureMonths": 180
}
```

10. Approve Loan
`PATCH /api/loans/1/decision`
```json
{
  "isApproved": true,
  "remarks": "Credit score verified"
}
```
