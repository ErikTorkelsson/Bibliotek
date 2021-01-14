# Centrumbibliotekets API


Cetrumbibliotekets API är ett webb API med en simpel vy som ger dig information om låntagare med försenade lån. 
APIns uppgift är att agera mellanhand mellan en webbsida och en databas. APIet tar både emot och skickar information mellan båda parterna, beroende på om man vill hämta ut eller spara/redigera data.

# Funktioner
##### Färdiga metoder för:
 - Lån av bok
 - Returnering av bok

##### Tar emot och hantarar data för:
  - Böcker
  - Författare
  - Lånekort och låntagare
  - Lån av böcker
 
##### Vy för försenade lån
Med i programmet ingår en webbaserad vy som kollar om det finns försenade lån och visar dem som en lista.
Vyn innehåller:

-  Lista med alla låntagare som har böcker som ej lämnats tillbaks innan returdatum
-  Lista inehållande alla böcker som ej lämnats tillbaks innan returdatum
-  Specifik lista för låntagare som visar alla dess försenade böcker
-  Information om låntagaren som behövs för kontakt


# Användning

##### Vy

För att komma åt listan krävs endast att du sufar in på utsatt webblänk och en lista med låntagare kommer att vara det första du ser. På sidan finns även en länk till alla försenade lån, samt en länk efter varje person i listan, som kommer att visa en lista av alla deras försenade böcker. 

URL för vyn kommer att bestämmas vid produktionssättning. Instruktioner för detta medföljer i separat dokument. 

##### API
API:et tar emot data av typen JSON med CRUD(Create, Read, Update, Delete) funktionalitet. 
Det finns alltså möjlighet att Skapa data, läsa av data, ändra existerande data och ta bort data som följer strukturen enligt programmets modeller. 

# Teknik

Programmet är utvecklat i Visual Studio med språket C#.

Projectet är skapat i asp.net Core version 3.1 Med Entity Framework.

Kodens Upplägg är baserat på entity frameworks "Code First" Vilket innebär att 
Databasen byggs efter hur modellerna i Apiet är uppbyggt.

### Instruktioner

##### Modeller 

I projektet finns en modell för varje Entitet, dvs Bok, författare, lånekort, lån, index samt en modell för kopplingstabbelen mellan bok och författare. Kopplingstabellen BookAuthor är den enda many to many relationen i API:et, och eftersom vi använder v.3.1 av asp.net så sätts relationens nycklar I modelbuilder metoden i dbcontext filen. De andra modellerna har one to many relationer och nycklarna skapas automatiskt genom navigations propertiesen som ligger i modellerna. 

![](Images/erd.png)

##### Controllers 

Alla modeller i projektet har varsin API controller med CRUD funktionallitet. Alla är scuffoldade med Entity Framework och ser i princip likadana ut. De skickar ut och tar emot JSON data. 

I Card controllern ligger dock två handskrivna metoder för att låna och lämna tillbaks böcker. Lånemetoden kollar om boken är tillgänglig, och är den det skapar en ett nytt lån och sätter ut lånedatum sam datum då boken skall vara tillbaks 

Returmetoden kollar om boken är utlånad och sätter sedan ett returdatum. 

Det finns även en separat controller för den inbyggda webb vyn som heter “Rented”. Som plockar upp data för de olika listorna som kan visas på sidan. 

##### ConnectionString 

API:ets Connectionstring är utsatt till en lokal databas med Windows authentication. Den kommer att behöva ändras beroende på vilken typ av databas som ska användas. Om databasen har en egen användare med lösenord måste det också finnas med i API:ets connectionstring.

### Produktionssättning 

Detta är ett webb baserat API med en Vy byggd för webbläsare och det är därför lämpligt att ladda upp projektet till en webb server 

Detta kan göras på flera sätt. Men för våran typ av projekt är den simplaste vägen förmodligen att ladda upp projektet via Azure. 

Azure är Microsofts molntjänst som har gjort det väldigt simpelt att ladda upp projektet direkt genom Visual Studio. 

För att göra detta behöver du först skapa ett Azure konto, detta kan du göra gratis på deras sida: 

https://azure.microsoft.com/sv-se/free/dotnet/ 

 

När du sedan har ett Azure konto kommer du att behöva skapa en Azure service instans. Detta kan du göra genom azure portal från deras hemsida https://portal.azure.com 

Välj “Create a resourse” i menyn och sedan app service. 

Fyll i Uppgifterna som behövs samt välj mängden minne och lagring du behöver för din server. Projektet är av typen .Net Core version 3.1 Vilket ska fyllas i  runtime stack. 

 

När detta är klart kan du publicera ditt projekt direkt från Visual Studio. Detta kan du göra genom att högerklicka på ditt projekt i Visual Studio och sedan Publish > Azure >  och välj sedan din app service. Om allting fungerat som det ska bör din sida ligga uppe på https://valtnamn.azurewebsites.net/. 

För att kunna börja arbeta med APIet behöver vi också få upp en databas som det kan jobba med. Man kan med Azure även smidigt skapa en molnbaserad sql-databas på liknade sätt som vi skapade våran app service. Bra dokumentation för att göra detta: 

https://docs.microsoft.com/sv-se/azure/azure-sql/database/single-database-create-quickstart?tabs=azure-portal 

Kom ihåg att för att du behöver ändra API:ets connectionstring i appsettings filen till den nya databasen. Denna går att hitta i informationen om din databas i Azure portal. 

Om ni smidigt vill kunna uppdatera sidan rekommenderar jag även att använda er utav Azure dev-ops och koppla upp er med Github-repot. På så vis kan ni automatisera uppdateringarna så att det sköter sig själv varje gång den nya koden pushas till Github: 

https://docs.microsoft.com/en-us/azure/devops/user-guide/code-with-git?view=azure-devops 

Lycka till! 
