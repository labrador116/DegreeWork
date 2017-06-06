using DegreeWork.ChromosomeModel;
using DegreeWork.Container;
using DegreeWork.Instances;
using DegreeWork.Model.Instances;
using DegreeWork.Service;
using DegreeWork.SpaceParam;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DataBaseStruct.Model;

namespace DegreeWork.GeneticAlgorithm
{
   public class GA
    {
       
        /*Проверка пересечений в процессе работы алгоритма*/
        public static Hashtable CheckIntersection(Chromosome chromosome, int val)
        {
           Hashtable hashMap = new Hashtable();
         
            int length = chromosome.Container.Count;
            //Количество контрольных точек
            int countOfPoint = SingleSpaceParams.getInstance().ControlPoints.Count;
            //Количество комнат
            int countofRoom = SingleSpaceParams.getInstance().Rooms.Count;

            List<ControlPointInst> points = new List<ControlPointInst>(SingleSpaceParams.getInstance().ControlPoints);
            List<RectangleRoom> rooms = new List<RectangleRoom>(SingleSpaceParams.getInstance().Rooms);

            for (int i = 0; i < length; i++)
            {
                //Проврека на покрытие сигналом контрольных точек, если все точки уже покрыты, тогда проерка не проводится
                if (countOfPoint != 0)
                {
                    if (checkOfCoverageOfControlPoint(chromosome.Container.ElementAt(i), points))
                    {
                        countOfPoint--;
                    }
                    else
                    {
                        hashMap.Add("point", points.First());
                        hashMap.Add("gene", chromosome.Container.ElementAt(i));
                        return hashMap;
                    }
                }

                /*Проверка покрытия комнат сигналом. Если все контрольные точки уже покрыты, а комнаты нет, и остались устройства беспроводной связи 
                 * тогда последующие гены будут размещается до тех пор, пока комнаты не будут покрыты*/
                if (countofRoom != 0)
                {
                    if (checkOfCoverageOfRoom(chromosome.Container.ElementAt(i), rooms))
                    {
                        countofRoom--;
                    }
                    else
                    {
                        if (countofRoom == 0 && i != length && countOfPoint==0)
                        {
                            hashMap.Add("room", rooms.First());
                            hashMap.Add("gene", chromosome.Container.ElementAt(i));
                            return hashMap;
                        }
                    }
                }

                for (int j = 0; j < length; j++)
                {
                    //ToDo Самая первая гена не проверяется на пересечения с границами, нужно исправить.
                    if (j != i)
                    {
                        bool resultValue = AlgorithmOfCheckIntersection(
                            chromosome.Container.ElementAt(i),
                            chromosome.Container.ElementAt(j)
                            );

                        if (resultValue == true)
                        {
                            hashMap.Add("coverage", 1);
                            hashMap.Add("gene", chromosome.Container.ElementAt(j));
                            return hashMap;
                        }
                        else
                        {
                            // Самый первый ген (окружность) получает зону покрытия равной своей площади
                            if (i == 0)
                            {
                                Gene gene = chromosome.Container.ElementAt(i);
                                chromosome.Container.ElementAt(i).CoverageOfArea = Math.PI * (gene.Radius * gene.Radius);
                            }
                            else
                            {
                                //Условие оценки: Оценивается пересекающая окружность, а не пересекаемая.
                                if (j < i)
                                {
                                    //отправляем i=B, j=A
                                    AlghoritmOfSettingCovarage(chromosome.Container.ElementAt(j),
                                      chromosome.Container.ElementAt(i)
                                      );
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /*Проверка пересечений и вычисление площади покрытия для создания первой популяции.
         Проверка покрытия плошади на обязательных точках. Необходимо сделать, что бы сначала выполнялось размещение 
         с учетом точек*/
        public static void CheckIntersection(Chromosome chromosome)
        {
            int length = chromosome.Container.Count;
            //Количество контрольных точек
            int countOfPoint = SingleSpaceParams.getInstance().ControlPoints.Count;
            //Количество комнат
            int countofRoom = SingleSpaceParams.getInstance().Rooms.Count;
            int counter = 0;
            List<ControlPointInst> points = new List<ControlPointInst>(SingleSpaceParams.getInstance().ControlPoints);
            List<RectangleRoom> rooms = new List<RectangleRoom>(SingleSpaceParams.getInstance().Rooms);
            int from = 0;
            int to = 0;
            for (int i = 0; i <length ; i++)
            {
                //Проврека на покрытие сигналом контрольных точек, если все точки уже покрыты, тогда проерка не проводится
                if (countOfPoint != 0)
                {
                    if (checkOfCoverageOfControlPoint(chromosome.Container.ElementAt(i), points.ElementAt(i)))
                    {
                        countOfPoint--;
                    }
                    else
                    { 
                        if (chromosome.Container.ElementAt(i).OX> points.ElementAt(i).X1)
                        {
                            if (to == 0 || chromosome.Container.ElementAt(i).OX < to)
                            { to = chromosome.Container.ElementAt(i).OX; }
                            ExecuteService.RefactorBadGeneForWidthTo(chromosome.Container.ElementAt(i), to);
                        }
                        else
                        {
                            //ExecuteService.RefactorBadGene(chromosome.Container.ElementAt(i));
                                  if (from == 0 || chromosome.Container.ElementAt(i).OX > from)
                                  { from = chromosome.Container.ElementAt(i).OX; }
                                  ExecuteService.RefactorBadGeneForWidthFrom(chromosome.Container.ElementAt(i), from);
                        }
                
                        //ExecuteService.RefactorBadGene(chromosome.Container.ElementAt(i));
                        if (i == 0)
                        {
                            i = -1;
                        }
                        else
                        {
                            i--;
                        }
                        continue;
                    }
                }

                /*Проверка покрытия комнат сигналом. Если все контрольные точки уже покрыты, а комнаты нет, и остались устройства беспроводной связи 
                 * тогда последующие гены будут размещается до тех пор, пока комнаты не будут покрыты*/
                if (countofRoom != 0)
                {
                    if (checkOfCoverageOfRoom(chromosome.Container.ElementAt(i), rooms.ElementAt(0)))
                    {
                        countofRoom--;
                        rooms.Remove(rooms.ElementAt(0));
                    }
                    else
                    {
                        if (i!=length && countOfPoint==0)
                        {
                            ExecuteService.RefactorBadGene(chromosome.Container.ElementAt(i));
                            if (i == 0)
                            {
                                i = -1;
                            }
                            else
                            {
                                i--;
                            }
                            continue;
                        }
                    }
                }

                for (int j = 0; j < length; j++)
                {
                    to = 0;
                    from = 0;
                    //ToDo Самая первая гена не проверяется на пересечения с границами, нужно исправить.
                    if (j != i)
                    {
                        bool resultValue = AlgorithmOfCheckIntersection(
                            chromosome.Container.ElementAt(i),
                            chromosome.Container.ElementAt(j)
                            );
                        
                        if (resultValue == true)
                        {
                            ExecuteService.RefactorBadGene(chromosome.Container.ElementAt(j));
                            j = -1;
                        }
                        else
                        {
                            // Самый первый ген (окружность) получает зону покрытия равной своей площади
                            if (i == 0)
                            {
                                Gene gene = chromosome.Container.ElementAt(i);
                                chromosome.Container.ElementAt(i).CoverageOfArea = Math.PI * (gene.Radius * gene.Radius);
                            }
                            else
                            {
                                //Условие оценки: Оценивается пересекающая окружность, а не пересекаемая.
                                if (j < i)
                                {
                                    //отправляем i=B, j=A
                                    AlghoritmOfSettingCovarage(chromosome.Container.ElementAt(j),
                                      chromosome.Container.ElementAt(i)
                                      );
                                }
                            }
                        }
                    }
                }  
            }
        }

        /*Метод проверки на вхождение точки центра окружности в область прямоугольника*/
        private static bool checkOfCoverageOfRoom(Gene gene, List<RectangleRoom> rooms)
        {
            foreach (RectangleRoom room in rooms)
            {
                if (gene.OX > room.X1 && gene.OX<room.X2 && gene.OY > room.Y1 && gene.OY < room.Y4)
                {
                    rooms.Remove(room);
                    return true;
                }
            }
            return false;
        }

        private static bool checkOfCoverageOfRoom(Gene gene, RectangleRoom room)
        {
                if (gene.OX > room.X1 && gene.OX < room.X2 && gene.OY > room.Y1 && gene.OY < room.Y4)
                {
                    return true;
                }
            return false;
        }

        /*Метод проверки вхождения точек в область окружноти*/
        private static bool checkOfCoverageOfControlPoint(Gene gene,List<ControlPointInst> points)
        {
            foreach(ControlPointInst point in points)
            {
                double F = Math.Pow(point.X1 - gene.OX, 2) + Math.Pow(point.Y1 - gene.OY, 2);

                if (F <= Math.Pow(gene.Radius, 2))
                {
                    points.Remove(point);
                    return true;
                }
            }
            return false;
        }

        private static bool checkOfCoverageOfControlPoint(Gene gene, ControlPointInst point)
        {
            
                double F = Math.Pow(point.X1 - gene.OX, 2) + Math.Pow(point.Y1 - gene.OY, 2);

                if (F <= Math.Pow(gene.Radius, 2))
                {
                    return true;
                }
            return false;
        }

        /*Алгоритм оценки покрываемого пространства. Оценивается ген B.*/
        private static void AlghoritmOfSettingCovarage(Gene A, Gene B)
        {
            /*Длина прямой от центра первого круга до центра второго круга*/
            double radiusLenght = Math.Sqrt(
                ((A.OX - B.OX) * (A.OX - B.OX)) +
                ((A.OY - B.OY) * (A.OY - B.OY))
                );

            //Если пересечения нет, то площадь покрываемого пространства равна площади круга
            if(radiusLenght >= (A.Radius + B.Radius))
            {
                B.CoverageOfArea = Math.PI * (B.Radius*B.Radius);
                return;
            }
            else
            {
                double F1 = 2 * Math.Acos(
                    (Math.Pow(A.Radius, 2) - Math.Pow(B.Radius, 2) + Math.Pow(radiusLenght, 2))
                    /
                    (2 * A.Radius * radiusLenght)
                    );

                double F2 = 2 * Math.Acos(
                    (Math.Pow(B.Radius, 2) - Math.Pow(A.Radius, 2) + Math.Pow(radiusLenght, 2))
                    /
                    (2 * B.Radius * radiusLenght)
                    );

                //Вычисление площадей секторов окружностей
                double S1 = (Math.Pow(A.Radius, 2) * (F1 - Math.Sin(F1))) / 2;
                double S2 = (Math.Pow(B.Radius, 2) * (F2 - Math.Sin(F2))) / 2;

                B.CoverageOfArea = (Math.PI * Math.Pow(B.Radius,2))- (S1 + S2);
            }

        }
        private static bool AlgorithmOfCheckIntersection(Gene A, Gene B)
        {
            int width = SingleSpaceParams.getInstance().Width;
            int height = SingleSpaceParams.getInstance().Height;
            /*Длина прямой от центра первого круга до центра второго круга*/
            double radiusLenght=Math.Sqrt(
                ((A.OX - B.OX)*(A.OX - B.OX))+
                ((A.OY-B.OY)*(A.OY - B.OY))
                );
            /*
             * Данное условие провярет на:
             * -Пересечение двух окружностей не более, чем на половину радиуса каждого круга.
             * -Размещение одной окружности внутри другой окружности
             * -Размещение окружностей относительно границ плоскости
             */
            //ToDo проверка на размеры плоскости индивидуально по хромосоме
            if (
                (Convert.ToInt32(radiusLenght) >= ((A.Radius / 2) + (B.Radius / 2))) &&
                !(Convert.ToInt32(radiusLenght) < A.Radius - B.Radius) &&
                ((B.OX-A.Radius)>=0 && (B.OX+B.Radius <= width)) &&
                ((B.OY-B.Radius)>=0 && (B.OY+B.Radius <= height))
               )
            {
                return false;
            }
            else
            {
                //Недопустимое размещение
                return true;
            }
        }
        
        /*Оценка фитнесс-функции*/
        public static double EvaluationOfFitenssFunc(Chromosome chromosome)
        {
            double sumOfGenesArea = 0;

            foreach (Gene gene in chromosome.Container)
            {
                sumOfGenesArea += gene.CoverageOfArea;
            }

            return (sumOfGenesArea / (SingleSpaceParams.getInstance().Width*SingleSpaceParams.getInstance().Height));
        }

        //Кроссинговер
        public static void CrossingOver(List<Chromosome> chromosomesContainer)
        {
            int sumChromosome = chromosomesContainer.Count;

            for (int i=0;i<sumChromosome; i = i + 2)
            {
                int dotOfCrossingOver = new Random().Next(0, chromosomesContainer.ElementAt(i).Container.Count);

                Chromosome chrA = chromosomesContainer.ElementAt(i);
                Chromosome chrB = chromosomesContainer.ElementAt(i + 1);
                //Создание потомков
                Chromosome child_one = OperationCO(chrA, chrB, dotOfCrossingOver);
                Chromosome child_two = OperationCO(chrB, chrA, dotOfCrossingOver);

                //Устранение недопустимостей
                checkInvalid(child_one, chrA, chrB);
                checkInvalid(child_two, chrB, chrA);

                //Устранение незаконности
                checkLegalDecision(child_one, chrA, chrB);
                checkLegalDecision(child_two, chrB, chrA);

                //Добавление потомков в контейнер
                chromosomesContainer.Add(child_one);
                chromosomesContainer.Add(child_two);
            }
        }

         //Операция кроссинговера
        private static Chromosome OperationCO(Chromosome chromA, Chromosome chromB, int dotOfCross)
        {
            Chromosome childChromosome = new Chromosome();

            for (int i = 0; i< chromA.Container.Count; i++)
            {
                if (i <= dotOfCross)
                {
                    childChromosome.Container.Add(
                        chromA.Container.ElementAt(i)
                        );
                }
                if (i > dotOfCross)
                {
                    childChromosome.Container.Add(
                        chromB.Container.ElementAt(i)
                        );
                }
            }

            return childChromosome;
        }

        //Проверка на недопустимость
        private static void checkInvalid(Chromosome child_one, Chromosome chrA, Chromosome chrB)
        {
            object resultIsLegitimate = isInvalid(child_one);

            while (resultIsLegitimate != null)
            {
                int value = (int)resultIsLegitimate;

                child_one.Container.RemoveAt(value);
                child_one.Container.Insert(value, chrA.Container.ElementAt(value));

                resultIsLegitimate = isInvalid(child_one);

                if (resultIsLegitimate != null && value == (int)resultIsLegitimate)
                {
                    foreach (Gene gene in chrB.Container)
                    {
                        if (child_one.Container.Contains(gene) == false)
                        {
                            child_one.Container.RemoveAt(value);
                            child_one.Container.Insert(value, gene);
                            break;
                        }
                    }
                }
                resultIsLegitimate = isInvalid(child_one);
            }
        }

        private static object isInvalid (Chromosome chromosome)
        {
            for (int i=0; i < chromosome.Container.Count; i++)
            {
                for (int j =0; j < chromosome.Container.Count; j++)
                {
                    if (i != j)
                    {
                        if (chromosome.Container.ElementAt(i).OX == chromosome.Container.ElementAt(j).OX 
                            && chromosome.Container.ElementAt(i).OY == chromosome.Container.ElementAt(j).OY)
                        {
                            return j;
                        }
                    }
                }
            }
            return null ;
        }

        //Проверка на законность решения. Если нет, то создается новая хромосома.
        private static void checkLegalDecision(Chromosome child, Chromosome parentA, Chromosome parentB)
        {
            int count = 0;
            Hashtable hashMap = CheckIntersection(child, 0);
            while (hashMap != null)
            {
              
                    if (hashMap.ContainsKey("point"))
                    {
                       // ControlPointInst point = (ControlPointInst)hashMap["point"];
                        Gene badGene = (Gene)hashMap["gene"];
                        int position = child.Container.IndexOf(badGene);
                        ExecuteService.RefactorBadGene(child.Container.ElementAt(position));
                        hashMap = CheckIntersection(child, 0);
                        continue;
                    }
                    if (hashMap.ContainsKey("room"))
                    {
                       // RectangleRoom room = (RectangleRoom)hashMap["room"];
                        Gene badGene = (Gene)hashMap["gene"];
                        int position = child.Container.IndexOf(badGene);
                        ExecuteService.RefactorBadGene(child.Container.ElementAt(position));
                        hashMap = CheckIntersection(child, 0);
                        continue;
                    }

                    if (hashMap.ContainsKey("coverage"))
                    {
                        /*Thread.Sleep(100);
                        int dotOfCrossingOver = new Random().Next(0, parentA.Container.Count);
                        child = OperationCO(parentA, parentB, dotOfCrossingOver);
                        checkInvalid(child, parentA, parentB);
                        child = OperationCO(parentA, parentB, dotOfCrossingOver);
                        hashMap = CheckIntersection(child, 0);*/

                       // RectangleRoom room = (RectangleRoom)hashMap["room"];
                        Gene badGene = (Gene)hashMap["gene"];
                        int position = child.Container.IndexOf(badGene);
                        ExecuteService.RefactorBadGene(child.Container.ElementAt(position));
                        hashMap = CheckIntersection(child, 0);
                    }
                   
                    /*Thread.Sleep(100);
                    int dotOfCrossingOver = new Random().Next(0, parentA.Container.Count);
                    child = OperationCO(parentA, parentB, dotOfCrossingOver);
                    checkInvalid(child, parentA, parentB);
                    hashMap = CheckIntersection(child, 0);*/
 
            }

        }

        /*Метод описывающий процесс мутации*/
        public static void Mutation(Chromosome chr)
        {
            var random = new Random();
            
            if (random.NextDouble() < SingleSpaceParams.getInstance().PropabilityOfMutation)
            {
                while (CheckIntersection(chr, 0) != null)
                {
                    int breakPoint = new Random().Next(0, chr.Container.Count - 1);
                    List<Gene> genesMove = new List<Gene>();
                    int counter = 0;
                    foreach (Gene gene in chr.Container)
                    {
                        if (counter < breakPoint)
                        {
                            genesMove.Add(gene);
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    foreach(Gene gene in genesMove)
                    {
                        chr.Container.Remove(gene);
                        chr.Container.Add(gene);
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
