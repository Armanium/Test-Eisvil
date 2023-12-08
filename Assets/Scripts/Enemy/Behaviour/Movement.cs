using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Behaviour
{
    public Enemy parent;
    public virtual void Move(Enemy.MovementTypes movementType,Transform transform, float speed, List<Tile> waypoints)
    {
        switch(movementType)
        {
            case Enemy.MovementTypes.run:

                Run(transform, speed, waypoints);

                break;

            case Enemy.MovementTypes.fly:

                Fly(transform, speed, waypoints);

                break;

            default:

                Debug.Log("Не найден такой вид передвижения");
                return;
        }
    }
    public virtual void Run(Transform transform, float speed, List<Tile> waypoints)
    {
        if (waypoints == null || waypoints.Count == 0) return;

        if(transform.position != waypoints[0].transform.position)
        {

            Vector3 position = Vector3.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
            transform.position = position;

            Vector3 direction = waypoints[0].transform.position;
            direction.y = transform.position.y;

            transform.LookAt(direction);
        }
        else waypoints.Remove(waypoints[0]);
    }

    public virtual void Fly(Transform transform, float speed, List<Tile> waypoints)
    {
        if(waypoints == null || waypoints.Count == 0) return;

        if (!waypoints[0].isObstacle)
        {
            if (transform.position != waypoints[0].transform.position)
            {
                Vector3 position = Vector3.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
                transform.position = position;

                Vector3 direction = waypoints[0].transform.position;
                direction.y = transform.position.y;

                transform.LookAt(direction);
            }
            else
            {
                waypoints.RemoveAt(0);
            }
        }
        else
        {
            if(transform.position != waypoints[1].transform.position)
            {
                if (parent.flytime > parent.flypathSpeed) parent.flytime = parent.flypathSpeed;

                float elapsed = parent.flytime / parent.flypathSpeed;
                float y = parent.flypath.Evaluate(elapsed);
                Debug.Log("Eval val:" + parent.flypath.Evaluate(elapsed));
                float height = Mathf.Lerp(0f, 1.5f, y);

                transform.position = Vector3.Lerp(transform.position, waypoints[1].transform.position, elapsed) + new Vector3(0,height,0);
            }
            else
            {
                waypoints.RemoveRange(0, 2);
                parent.flytime = 0;
            }

            parent.flytime += Time.deltaTime;
        }
    }

    public virtual List<Tile> GetMovePath(GameManager gameManager,Tile origin, Tile destination, float range)
    {
        return gameManager.GetMoveablePath(origin);
    }

    public virtual List<Tile> GetFlyPath(GameManager gameManager, Tile origin, Tile destination)
    {
        return gameManager.GetFlyablePath(origin);
    }
}
