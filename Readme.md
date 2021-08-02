 <h1>Suggest Music</h1>
<br/>
  <h3>Execução do projeto no docker</h3>
  <br/>
    <ul>
      <li>
        Verificar se o Docker esta instalado, <strong>e em execução</strong>
      </li>
      <li>
        Acessar via cmd a pasta raiz da solução ..\DesafioSIAGRI
      </li>
      <li>
        Executar o comando: <strong>docker build -t suggestmusic .</strong>
      </li>
      <li>
        Executar o comando:
        <strong>docker container run -it --rm -p 5000:80 --name
          suggestmusiccontainer suggestmusic
        </strong>
      </li>
      <li>
        Após executar os comandos acima. Acessar:
        <strong>
          <a href="http://localhost:5000/swagger">
            Link documentação desta API
          </a>
        </strong>
      </li>
    </ul>
