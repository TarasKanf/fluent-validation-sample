Testing JSON sample 

POST https://localhost:5001/family
{
    "FirstName": "myName",
    "LastName": "name",
    "MainChild": {
        "FirstName": "mainChildName",
        "LastName": "lastChildName",
        "Age": 10
    },
    "Children": [
        {
            "FirstName": "mainChildName",
            "LastName": "lastChildName",
            "Age": 10
        }
    ]
}