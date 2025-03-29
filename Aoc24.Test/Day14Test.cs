namespace Aoc24.Test;

public class Day14Test
{
    private const string Example = """
        p=0,4 v=3,-3
        p=6,3 v=-1,-3
        p=10,3 v=-1,2
        p=2,0 v=2,-1
        p=0,0 v=1,3
        p=3,0 v=-2,-2
        p=7,6 v=-1,-3
        p=3,0 v=-1,-2
        p=9,3 v=2,3
        p=7,3 v=-1,2
        p=2,4 v=2,-3
        p=9,5 v=-3,-3
        """;

    [Test]
    public async Task Part1()
    {
        // Arrange
        var day14 = new Day14(new StringReader(Example), 11, 7, 100, null);

        // Act
        var part1 = await day14.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(12);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        await using var treePrinter = new StringWriter();
        using var reader = File.OpenText("./Data/Day14");
        var day14 = new Day14(reader, 101, 103, 100, treePrinter);

        // Act
        await day14.Part2();

        // Assert
        await Assert.That(treePrinter.ToString()).IsEquivalentTo(Expected.Tree);
    }
}

file static class Expected
{
    public static readonly string Tree = """
                                      *                                   *                                  
                                                         *              *                                    
                                                                                *                            
                                                                                              *  *           
                                                                                                             
                           *                                                                           *     
         *      *                  *                                                                         
                                                                                    *   *                    
                                                                                 *                           
                                                                                                             
                                                                                                             
                                                *                                                         *  
                                        *                              *                                     
                                 * *             *           *                                               
                                          *                                                                  
              *                                                                                              
                                                                                                             
                                                                      *                                      
                                *                                                                            
                                                                                                             
                                      *  *                                                                   
                                                                                                             
                                                              *           *                                  
                                                                                                             
                               *                                                                             
                    *  *                   *                                  *                              
                  *                                                            *                             
                                                                                                             
                        *                                                                                    
               *              *               *******************************                                
                       *                      *                             *            *                   
                          *                   *                             * *                              
                                              *                             *                                
            *                                 *                             *                                
                                            * *              *              *  *      *                      
                                              *             ***             *                    *           
                                              *            *****            *                                
                                              *           *******           *     *         *                
                                              *          *********          *                     *          
                                              *            *****            *                                
                                 *            *           *******           *               *           *    
                                              *          *********          *                                
                                              *         ***********         *                          *     
                                    *         *        *************        *                                
               *                              *          *********          *                                
        *                                 *   *         ***********         *                                
                                              *        *************        *                                
                                              *       ***************       * *                              
                   *            *    *        *      *****************      *                                
                       *                      *        *************        *                                
           *                                  *       ***************       *                                
                                          *   *      *****************      *                                
                                              *     *******************     *                                
                     *              *         *    *********************    *                                
              *      *                        *             ***             *                                
                                              *             ***             *                        *       
                                              *             ***             *                                
                      *                       *                             *                         *      
                                        *     *                             *              *         *       
                                  *           *                             *                                
                  *                  *   *    *                             *                                
                                              *******************************                                
                                                                                                          *  
                                *                                                                            
                                                    *                                                        
                                                              *                                              
                 *                                                    *                                      
              *            *                                       *                  *                      
                                                                                                             
                                                                                                             
                                             *                                               *               
                                                                                    *                        
                                                                     *                                       
                                               *                                                             
                          *    *      *                                  *                                   
                      *                                                                                      
                              *                                                                           *  
                                                                                                         *   
                                      *                                                                      
                          *            *                                     *                        *  *   
                        *                                                                                    
                                                                              *    *                         
                                                                                                             
                                                                                                             
                                                                                                             
         *                                            *        *                                             
                                                                                                        *    
           *                                 *                                   *      *                    
             *                                                          *       *   *                        
                              *                                                                              
                                                                    *                                        
                                                                                            *                
                                                                *            *  *                            
                                                                                                             
            *                     *                                    *                     *               
                                                                                     *                       
                                                     *                                                       
                     *                                                                                       
                                                               *               *                             
                     *                                     *                                                 
                                  **                                                              *          
                                                      *                                                      
                                                                                                             
        """.ReplaceLineEndings("\n");
}
