-> Logs disponiveis no grafana cloud
 - Configuracoes do grafana cloud estão no arquivo .env
-> Serviço de armazenamento de imagem escolhido foi o cloudinary

//Comandos migrations
 - Gera Migrations
dotnet ef migrations add <nomeMigration> 
 - Gera script
dotnet ef migrations script -o Scripts/<nomeArquivo>.sql