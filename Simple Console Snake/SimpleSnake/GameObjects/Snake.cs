using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleSnake.GameObjects
{
    public class Snake
    {
        private const char SnakeSymbol = '\u25CF';
        private const char EmptySpace = ' ';

        private Queue<Point> snakePartsPosition;
        private Food[] food;
        private Wall wall;
        private int nextLeftX;
        private int nextTopY;
        private int foodIndex;

        public Snake(Wall wall)
        {
            this.snakePartsPosition = new Queue<Point>();
            this.food = new Food[3];
            this.wall = wall;
            this.foodIndex = RandomFoodNumber;
            this.GetFoods();
            this.CreateSnake();
            this.AddRandomFood();
        }

        private void AddRandomFood()
        {
            Food currentFood = this.food[foodIndex];

            for (int i = 0; i < 1; i++)
            {
                currentFood.SetRandomPosition(snakePartsPosition);
            }
        }

        public int RandomFoodNumber => new Random().Next(0, this.food.Length);

        private void CreateSnake()
        {
            for (int topY = 1; topY <= 6; topY++)
            {
                this.snakePartsPosition.Enqueue(new Point(2, topY));
            }
        }

        private void GetFoods()
        {

            this.food[0] = new FoodHash(this.wall);
            this.food[1] = new FoodDollar(this.wall);
            this.food[2] = new FoodAsterisk(this.wall);
        }

        private void GetNextPoint(Point direction, Point snakeHead)
        {
            this.nextLeftX = snakeHead.LeftX + direction.LeftX;
            this.nextTopY = snakeHead.TopY + direction.TopY;
        }

        public bool IsMoving(Point direction)
        {
            Point currentSnakeHead = this.snakePartsPosition.Last();

            GetNextPoint(direction, currentSnakeHead);

            bool isPointOfSnake = this.snakePartsPosition
                        .Any(x => x.LeftX == nextLeftX && x.TopY == nextTopY);

            if (isPointOfSnake)
            {
                return false;
            }

            Point snakeNewHead = new Point(this.nextLeftX, this.nextTopY);

            if (this.wall.IsPointOfWall(snakeNewHead))
            {
                return false;
            }
            
            this.snakePartsPosition.Enqueue(snakeNewHead);
            snakeNewHead.Draw(SnakeSymbol);

            if (food[foodIndex].IsFoodPoint(snakeNewHead))
            {
                this.Eat(direction, currentSnakeHead);
            }

            Point snakeTail = this.snakePartsPosition.Dequeue();
            snakeTail.Draw(EmptySpace);

            return true;
        }

        private void Eat(Point direction, Point currentSnakeHead)
        {
            int length = food[foodIndex].FoodPoints;

            for (int i = 0; i < length; i++)
            {
                this.snakePartsPosition.Enqueue(new Point(this.nextLeftX, this.nextTopY));
                GetNextPoint(direction, currentSnakeHead);
            }

            this.foodIndex = this.RandomFoodNumber;
            this.food[foodIndex].SetRandomPosition(this.snakePartsPosition);
        }
    }
}
