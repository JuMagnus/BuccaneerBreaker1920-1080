using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuccaneerBreaker
{
    public class Brick : Sprite
    {
        //Dans les prochaines améliorations, j'aimerais pouvoir ajouter de nouvelles textures afin d'avoir des niveaux plus agreables et complexes
        //avec de nouvelles couleurs. il faudra sans doute gérer ça au niveau des fichiers json. A réflechir
        public Brick(Vector2 position) : base(position)
        {
            _texture = ServicesLocator.Get<IAssets>().Get<Texture2D>("TexBrickBrown");
        }

        //A voir si je mets aussi ici les points de vie des briques ou si je gère ça au niveau du json. 


    }
}
