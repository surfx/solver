# Solver C#

Solver LCP (lógica clássica proposicional) de Tableaux KE

No momento foram implementados o parser, o print da árvore em string e em imagem png.

# Exemplo Parser

```
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

## Saídas

### Formato string

`Console.WriteLine(f.ToString());`

obs: tags OPEN/CLOSED apenas ilustrativas

```
                                           F C → E                            
                                           F A                                
                                           T A → B                            
                                           T C                                
                                           T A ˅ D                            
               F Y → (A ˅ B)                                     F A ˅ Z      
               T E                                               T H → G      
 T G ˄ (Y → B)               F G                                 T (G ˅ T) ˄ U
 OPEN                        T G ˄ (Y → B)                       T (G ˅ T) ˄ X
                             CLOSED                T (G ˅ T) ˄ U              
                                                   OPEN                       
```


### Imagem png

obs: tags OPEN/CLOSED apenas ilustrativas

```
    PFormulasToImage pf2img = PFormulasToImage.PFormulasToImageBuilder.Init(f)
            .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula.png"))
            .withDivisoriaArvore()
            .Build();
    new ImageFormulas().formulasToImage(pf2img);
```

<img src="imgformulas\bmp_formula.png" alt="Exemplo de árvore">

# Análises iniciais


| Árvores |        |         |        | Parser  |
| ------- | ------ | ------- | ------ | ------- |
|<a href="imagens\estudos\arv_001.jpg" target="_blank"><img src="imagens\estudos\arv_001.jpg" alt="Árvore Inicial" width="100" height="100"></a>|<a href="imagens\estudos\arv_002.jpg" target="_blank"><img src="imagens\estudos\arv_002.jpg" alt="Árvore Inicial" width="100" height="100"></a>|<a href="imagens\estudos\arv_003.jpg" target="_blank"><img src="imagens\estudos\arv_003.jpg" alt="Árvore Inicial" width="100" height="100"></a>|<a href="imagens\estudos\arv_004.jpg" target="_blank"><img src="imagens\estudos\arv_004.jpg" alt="Árvore Inicial" width="100" height="100"></a>|<a href="imagens\estudos\parser_001.jpg" target="_blank"><img src="imagens\estudos\parser_001.jpg" alt="Parser Inicial" width="100" height="100"></a>|

# Regras Tableaux KE

<img src="imagens\rules_KE_1.png" alt="Regras Tableaux KE">


# Exemplos de provas

Ainda não implementado. Imagens de exemplos retiradas do software KEMS e de material do professor doutor Adolfo Gustavo Serra Seca Neto.

| Prova 1 | Prova 2 |
| ------- | ------ |
|<a href="imagens\2-Figure1-1.png" target="_blank"><img src="imagens\2-Figure1-1.png" alt="Prova 1" width="100" height="100"></a>|<a href="imagens\2-Figure2-1.png" target="_blank"><img src="imagens\2-Figure2-1.png" alt="Prova 2" width="100" height="100"></a>|


# TODO

- [x] Estrutura de fórmulas - código C#, classes, etc
- [x] Parser
- [x] Print árvore string
- [x] Print árvore png
- [x] Stage, versão inicial \(stage: conjunto de fórmulas)
- [x] Separar a abstração do 'print árvore png' em uma classe parametrizada
- [ ] Regras binárias e unárias
- [ ] Aplicar conjunto de regras às fórmulas do stage
- [ ] Verificar se os ramos estão abertos ou fechados
- [ ] Verificar se a árvore possui solução ou não
- [ ] Interface externa do console, para entradas e saídas

# Libs

```
dotnet add package System.Drawing.Common --version 7.0.0
```

# Obs

Para executar: `CTRL + F5`

# Urls

- [google search](https://www.google.com/search?q=tableu+ke+proof&tbm=isch&ved=2ahUKEwjq2Zu77LT_AhXcrZUCHb0dDdUQ2-cCegQIABAA&oq=tableu+ke+proof&gs_lcp=CgNpbWcQA1DPA1icDGCQDWgAcAB4AIAB5wGIAbgKkgEDMi02mAEAoAEBqgELZ3dzLXdpei1pbWfAAQE&sclient=img&ei=82aCZKqUFtzb1sQPvbu0qA0&bih=1086&biw=2154&client=opera-gx&hs=Kn8#imgrc=4ioBaZZw7fOZwM)
- [símbolos alt](https://www.freecodecamp.org/portuguese/news/codigos-alt-como-digitar-caracteres-especiais-e-simbolos-do-teclado-no-windows-usando-as-teclas-alt/)
- [KEMS](https://github.com/adolfont/KEMS)

## Tutoriais Imagens

- [Text on Image](https://stackoverflow.com/questions/6826921/write-text-on-an-image-in-c-sharp)
- [Google search](t.ly/_m-Z)




