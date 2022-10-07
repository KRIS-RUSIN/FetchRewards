# FetchRewards

## Prerequisites
* Need .NET 5.0 installed (https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* Hoppscotch (or Postman installed)

## Routes/Endpoints
* GET /transactions
  * Returns a list of all transactions
  * No request body
* POST /transactions
  * Creates a transaction and adds it to memory
  * Request body: { "payer": string, "points": int, "timestamp": DateTime }
  * Returns a list of all transactions
* GET /transactions/balance
  * Returns the balance of all payer's points
  * No request body
* POST /transactions/spend
  * Spends points
  * Returns a list of balances that were deducted
  * Request body: { "points": int }
  
## Run
1. Open Command Prompt/Terminal
2. Navigate to folder where FetchRewards.sln is located
3. Execute command `dotnet run .\FetchRewards.sln` (On Mac if command does not work, run `ln -s /usr/local/share/dotnet/dotnet /usr/local/bin/` and try again)
4. Open Postman and enter `https://localhost:5001/` as the request url,
5. Select type of request such as GET and add correct route to the request url such as `/transactions` (EX: `https://localhost:5001/transactions`)
6. DONE!
