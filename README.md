### Example POST raw JSON

```json
{
  "date": "2025-05-22T00:00:00",
  "dueDate": "2026-06-01T00:00:00",
  "idDoctor": 2,
  "patient": {
    "firstName": "Hey",
    "lastName": "Bober",
    "birthdate": "1990-03-15T00:00:00"
  },
  "medicaments": [
    {
      "idMedicament": 2,
      "dose": 1,
      "details": "NO"
    },
    {
      "idMedicament": 1,
      "dose": 3,
      "details": "YES"
    }
  ]
}
