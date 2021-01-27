module ("Unity3dTools", package.seeall);

function TextureToSprite(Texture)
    local sprite = UnityEngine.Sprite.Create(Texture,UnityEngine.Rect.New(0,0,128,128), Vector2.zero);
    return sprite
end

