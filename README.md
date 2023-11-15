# Solver Tableau KE C# v1.0.0

Solver LCP (lógica clássica proposicional) de Tableau KE, versão 1.0.0

Solver em desenvolvimento, dúvidas, críticas, erros, sugestões são bem-vindas. 
O código é livre para uso, apenas peço a referência ao projeto.

Pode ser utilizado via linha de comando ou acesando diretamente as classes C#.

Libs necessárias: ver seção **Libs**

# Exemplo de uso (linha comando)

## Parâmetros aceitos


| Parâmetro         | Obrigatório | Tipo    | Descrição                                 |
| ----------------- | ----------- | ------- | ----------------------------------------- |
| file_formulas     | sim         | entrada | arquivo onde estão as fórmulas lógicas    |
| file_img          | não         | saída   | arquivo onde a árvore de prova será salva |
| file_estatisticas | não         | saída   | arquivo onde as estatíticas serão salvas  |

Exemplo de uso:

`$ dotnet run file_formulas=C:\Users\...\parser\formulas.txt file_img=C:\Users\...\parser\prova.png file_estatisticas=C:\...\Desktop\parser\estatisticas.txt`

Ou:

```
$ cd ...\solver_project\project\solver\bin\Debug\net7.0\
$ .\solver.exe file_formulas=C:\Users\...\parser\formulas.txt file_img=C:\Users\...\parser\prova.png file_estatisticas=C:\...\Desktop\parser\estatisticas.txt
```

ps: novos parâmetros estão em desenvolvimento

### Exemplo file_formulas

O arquivo `formulas.txt` de entrada deve ter o seguinte formato:

```
T (A | C -> E) | (!C | B & E)
T !E
T B
F C & D
F !D
F !A
```

# Exemplo Parser (code C#)

Para referência direta, a sintaxa base é:

```csharp
    Formulas f = new();

    Parser parser = new();
    f.addConjuntoFormula(parser.parserCF("A->B"));
    f.addConjuntoFormula(parser.parserCF("F C->E"));
    f.addConjuntoFormula(parser.parserCF("C"));
    f.addConjuntoFormula(parser.parserCF("F A"));
    f.addConjuntoFormula(parser.parserCF("T (A | D)"));

    f.addEsquerda(parser.parserCF("E"));
    f.addEsquerda(parser.parserCF("F Y -> (A | B)"));

    f.addDireita(parser.parserCF("T H->G"));
    f.addDireita(parser.parserCF("F (A|Z)"));
    f.addDireita(parser.parserCF("T G|T&U"));
    f.addDireita(parser.parserCF("T G|T&X"));
    f.Direita.addEsquerda(parser.parserCF("T G|T&U"));

    f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));
    f.Esquerda.addDireita(parser.parserCF("F G"));

    f.Esquerda.addEsquerda(parser.parserCF("G & (Y -> B)"));

    f.Esquerda.Direita.isClosed = true;
```

Um exemplo de uso pode ser encontrado na classe `classes.auxiliar.inputs.ExecucaoSolver.cs`

## Saídas

### Formato string

`Console.WriteLine(f.ToString());`

obs: tags OPEN/CLOSED apenas ilustrativas

```
                                              1 T A
                                              2 T A → B
                                              3 F B
     4 F A → B                                                               11 F A ˅ B
     5 T C                                                                   12 T C ˅ A
6 T C              8 F ((C → B) ˄ C) ˅ D                                     13 T (C ˅ A) → B
7 F C         9 T A                     10 T B                               14 T C
OPEN          OPEN                      OPEN           15 T C
                                                       16 F C
                                                       17 F ((C → B) ˄ C) ˅ D
                                                       CLOSED
```

### Imagem png

obs: tags OPEN/CLOSED apenas ilustrativas

```csharp
    PFormulasToImage pf2img = PFormulasToImage.PFormulasToImageBuilder.Init(f)
            .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula.png"))
            .withDivisoriaArvore()
            .Build();
    new ImageFormulas().formulasToImage(pf2img);
```

<img src="imgformulas\bmp_formula.png" alt="Exemplo de árvore">

# Estudos iniciais

| Árvores                                                                                                                                         |                                                                                                                                                 |                                                                                                                                                 |                                                                                                                                                 | Parser                                                                                                                                                |
| ----------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------- |
| <a href="imagens\estudos\arv_001.jpg" target="_blank"><img src="imagens\estudos\arv_001.jpg" alt="Árvore Inicial" width="100" height="100"></a> | <a href="imagens\estudos\arv_002.jpg" target="_blank"><img src="imagens\estudos\arv_002.jpg" alt="Árvore Inicial" width="100" height="100"></a> | <a href="imagens\estudos\arv_003.jpg" target="_blank"><img src="imagens\estudos\arv_003.jpg" alt="Árvore Inicial" width="100" height="100"></a> | <a href="imagens\estudos\arv_004.jpg" target="_blank"><img src="imagens\estudos\arv_004.jpg" alt="Árvore Inicial" width="100" height="100"></a> | <a href="imagens\estudos\parser_001.jpg" target="_blank"><img src="imagens\estudos\parser_001.jpg" alt="Parser Inicial" width="100" height="100"></a> |

# Regras Tableau KE

<img src="imagens\rules_KE_1.png" alt="Regras Tableau KE">

# Exemplos de provas

Ainda não implementado. Imagens de exemplos retiradas do software KEMS e de material do professor doutor [Adolfo Gustavo Serra Seca Neto](https://adolfont.github.io), atualmente docente na UTFPR - Curitiba/PR.

| Prova 1                                                                                                                          | Prova 2                                                                                                                          |
| -------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| <a href="imagens\2-Figure1-1.png" target="_blank"><img src="imagens\2-Figure1-1.png" alt="Prova 1" width="100" height="100"></a> | <a href="imagens\2-Figure2-1.png" target="_blank"><img src="imagens\2-Figure2-1.png" alt="Prova 2" width="100" height="100"></a> |

# TODO

- [x] Estrutura de fórmulas - código C#, classes, etc
- [x] Parser
- [x] Print árvore string
- [x] Print árvore png
- [x] Stage, versão inicial \(stage: conjunto de fórmulas)
- [x] Separar a abstração do 'print árvore png' em uma classe parametrizada
- [x] Regras binárias e unárias. Implementar Regras: F ˄1, F ˄2, T →1, T →2, T ˅1, T ˅2, F →, F ˅, T ˄, F ¬ e T ¬
- [x] Regra PB (Princípio da bivalência)
- [x] Regras beta: F ˄, T →, T ˅
- [x] Aplicar conjunto de regras às fórmulas do stage
- [ ] Implementar o conceito de estratégia
- [x] Verificar se os ramos estão abertos ou fechados
- [x] Verificar se a árvore possui solução ou não
- [ ] Valoração / contra-exemplo
- [x] Imprimir a árvore com o "número" da fórmula ao lado
- [x] Melhorar o print `toString()`
- [ ] Validar árvore e métodos de prova com mais exemplos
- [ ] Implementar estratégia para escolha da regra beta a ser aplicada
- [ ] Criar estratégia para cálculo de complexidade e variabilidade das fórmulas do stage
- [ ] Log, tempo de execução, memória utilizada
- [ ] Análisar tempos de prova, altura da árvore, complexidade, etc
- [x] Interface externa do console, para entradas e saídas
- [x] Print tree dot mode

# Libs

```
dotnet add package System.Drawing.Common --version 7.0.0
```

# Obs

Para executar vscode: `CTRL + F5` / `F5` ou `$ dotnet run`

# Urls

- [Tableau KE proof](https://www.google.com/search?q=tableu+ke+proof&tbm=isch&ved=2ahUKEwjq2Zu77LT_AhXcrZUCHb0dDdUQ2-cCegQIABAA&oq=tableau+ke+proof&gs_lcp=CgNpbWcQA1DPA1icDGCQDWgAcAB4AIAB5wGIAbgKkgEDMi02mAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=82aCZKqUFtzb1sQPvbu0qA0&bih=1086&biw=2154&client=opera-gx&hs=Kn8#imgrc=4ioBaZZw7fOZwM)
- [símbolos alt](https://www.freecodecamp.org/portuguese/news/codigos-alt-como-digitar-caracteres-especiais-e-simbolos-do-teclado-no-windows-usando-as-teclas-alt/)
- [Adolfo Gustavo Serra Seca Neto](https://adolfont.github.io)
- [KEMS](https://github.com/adolfont/KEMS)
- [Consumo memória C#](https://elemarjr.wordpress.com/2010/09/28/monitorando-consumo-de-memria-e-tempo-de-execuo/)

## Tutoriais Imagens

- [Text on Image](https://stackoverflow.com/questions/6826921/write-text-on-an-image-in-c-sharp)
- [C# console write text image](t.ly/_m-Z)
