using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNB
{
    /// <summary>
    /// 
    /// The main philosophy of the map generator is to restrict the user interface to this asset and the rest to be like a black box that,
    /// the most design profiles will not want to open because they feel that they have the necessary parameters to design and edit the most complex maps at a very high level of detail,
    /// and only programmers will modify. In this way errors produced by touching parts of the code without taking into account other parts that may be affected are avoided.
    /// Given this class is the core UI of our Map Generator, comments are included in all layout variables.
    /// 
    /// La filosofía principal del generador de mapas es restringir el interface del usuario a esta asset y que el resto sea como una black box que,
    /// los perfiles mas de diseño no querrán abrir al sentir que disponen de los parametros necesarios para diseñar y editar los mapas mas complejos a un altísimo nivel de detalle,
    /// y únicamente los programadores modificarán. De esta forma se evitan errores producidos por tocar partes del código sin tener en cuenta otras partes que puedan ser afectadas. 
    /// Dado lo nuclear de esta clase se incluyen comentarios en todas las variables de diseño.
    /// 
    /// 地图生成器的主要理念是将用户界面限制为该资源，其余部分就像一个黑匣子，
    /// 大多数设计师配置文件都不想打开，因为他们觉得自己拥有必要的参数来设计和编辑具有非常高的细节水平的最复杂的地图，
    /// 并且只有程序员才会修改。这可以防止在未考虑可能受影响的其他部分的情况下触及代码的某些部分时发生的错误。
    /// 鉴于此类的核心，注释包含在所有布局变量中。
    /// </summary>
    [CreateAssetMenu(fileName = "New Map", menuName = "MapSO")]
    [System.Serializable]
    public class MapSO : ScriptableObject
    {
        //Makes the modification of variables in this "asset" show the effects in the map in real time.
        //Hace que la modificación de variables en esta "asset" muestre los efectos en el mapa en tiempo real.
        //它使得这个“asset”中变量的修改可以实时地在地图上显示效果。
        public event System.Action OnValuesUpdated;
        public bool autoUpdate = true;

        //Set ENTER and EXIT MESH EDIT MODE using the buttons exposed in the editor (MeshEditosSebCNB.cs).
        //Configure ENTER y EXIT MESH EDIT MODE usando los botones expuestos en el editor (MeshEditosSebCNB.cs)
        //使用编辑器中公开的按钮设置 ENTER 和 EXIT MESH EDIT MODE(MeshEditosSebCNB.cs)
        [SerializeField]
        [HideInInspector]
        public bool inEditMode;

        //When entering MESH EDIT MODE this data structures will save the needes data to create a new Mesh,
        //edit this new Mesh And generate de colliders and Line Renderer components.
        //Al entrar al MODO DE EDICIÓN DE MALLA, estas estructuras de datos guardarán los datos necesarios para crear una nueva malla,
        //,editarla y generar colisionadores y componentes Line Renderer.
        //当进入MESH EDIT MODE时，这些数据结构将存储创建新网格所需的数据，编辑它并生成碰撞器和线渲染器组件。
        [SerializeField]
        [HideInInspector]
        public List<Vector3> verticesBackUp = new List<Vector3>();
        [SerializeField]
        [HideInInspector]
        public List<int> trianglesBackUp = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<Vector2> uvsBackUp;

        #region SOserialization problem with lists of lists
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo0 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo1 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo2 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo3 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo4 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo5 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo6 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo7 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo8 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo9 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo10 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo11 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo12 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo13 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo14 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo15 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo16 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo17 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo18 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo19 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo20 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo21 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo22 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo23 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo24 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo25 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo26 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo27 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo28 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo29 = new List<int>();
        [SerializeField]
        [HideInInspector]
        public List<int> outlinesBackUpInfo30 = new List<int>();
        #endregion

        //Sets the thickness of the walls surrounding the map.
        //Establece el grosor de las paredes que rodean el mapa.
        //设置地图周围墙壁的厚度。
        [Range(2, 10)]
        public int _borderMap = 2;
        //Sets the scriptable object that defines the 3 main material to set the look of the map (mesh, map surfaces and background).
        //Establece el scriptable object que define los 3 materiales principales para establecer el aspecto del mapa (mesh, superficies del mapa y fondo).
        //设置可编写脚本的对象，该对象定义 3 种主要材质以设置地图的外观（网格、地图表面和背景）。
        public MapMatSO _mapMatSO;

        public MapScriptO _map;

        //Class to set all the variables that define the generated map.
        //Clase para establecer todas las variables que definen el mapa generado.
        //用于设置定义生成的地图的所有变量的类
        [System.Serializable]
        public class MapScriptO
        {
            [Range(20, 1000)]
            public int width = 20;
            [Range(20, 1000)]
            public int height = 20;

            //This parameter sets the percentage of the wall tiles represented by 1 in the binary matrix created as the starting point with the width anf height before defined.
            //Este parámetro establece el porcentaje de paredes representados por 1 en la matriz binaria creada como punto de partida con el ancho y alto antes definidos.
            //此参数设置以上面定义的宽度和高度为起点创建的二进制数组中以 1 表示的墙的百分比。
            [Range(0, 55)]
            public int randomFillPercent = 45;

            //Sets the scale of the map
            //Establece la escala del mapa
            //设置地图的比例
            [Range(1, 4)]
            public int squareSize = 1;

            //All open areas must reachable, if not a tunnel is created to make the needed connections. This parameter sets the width of the tunnels.
            //Todas las áreas abiertas deben ser accesibles, de lo contrario, se crea un túnel para realizar las conexiones necesarias. Este parámetro establece el ancho de los túneles.
            //所有开放区域必须可访问，否则将创建隧道以进行必要的连接。该参数设置隧道的宽度。
            [Range(1, 5)]
            public int tunnelRadio = 1;


            //diferent seed, diferent map.
            //semilla diferente, mapa diferente.
            //不同的种子，不同的地图。
            [Range(0, 1000)]
            public int seed = 0;

            //low values ​​generate more abrupt and angular shapes on the map surfaces and high values ​​generate more rounded shapes.
            //valores bajos generan formas mas abruptas y anguladas en las superficies del mapa y valores altos formas mas redondeadas.            //不同的种子，不同的地图。
            //低值会在地图表面上生成更陡峭和有角度的形状，高值会生成更圆润的形状。
            [Range(0, 6)]
            public int smoothMapIterations = 5;

            //mesh
            [Range(1, 60)]
            public int meshTextureScale = 10;
            [Range(1, 60)]
            public int lineRendererTextureTilling = 35;
            [Range(0f, 1f)]
            public float lineRendererVertexSpacing = .1f;
            //................


            //spawners
            public List<FreeCellSpawnerScriptO> _freeCellSpawners;// in mapgen
            public List<ColliderSpawnerScriptO> _colliderSpawners;// in meshgen
        }

        [System.Serializable]
        public class FreeCellSpawnerScriptO
        {
            [Range(5, 20)]
            public float radius = 10;
            public bool addCollider = false;
            public bool colliderSimple = true; //colliderSimple tru is BoxCollider2d, false is PoligonCollider2d
            public float spritesScale = 1.0f;
            public List<Sprite> _sprites;
        }

        [System.Serializable]
        public class ColliderSpawnerScriptO
        {
            public bool colliderSimple = true; //colliderSimple tru is BoxCollider2d, false is PoligonCollider2d

            [Range(5, 200)]
            public int spawnDensityFloor = 20;
            public bool addCollidersFloor = false;

            [Range(5, 200)]
            public int spawnDensityTop = 20;
            public bool addCollidersTop = false;

            [Range(5, 200)]
            public int spawnDensityRest = 20;
            public bool addCollidersRest = false;


            public List<Sprite> spriteListFloor = new List<Sprite>();
            public float offsetFloor = 0;
            public float scaleFloor = .5f;

            public List<Sprite> spriteListTop = new List<Sprite>();
            public float offsetTop = 0;
            public float scaleTop = .5f;

            public List<Sprite> spriteListRest = new List<Sprite>();
            public float offsetRest = 0;
            public float scaleRest = .5f;
        }

#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (autoUpdate)
            {
                UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
            }
        }

        public void NotifyOfUpdatedValues()
        {
            UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
            if (OnValuesUpdated != null)
            {
                OnValuesUpdated();
            }
        }

#endif
    }
}