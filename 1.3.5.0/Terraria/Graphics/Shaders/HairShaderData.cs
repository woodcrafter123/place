﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Shaders.HairShaderData
// Assembly: TerrariaServer, Version=1.3.5.0, Culture=neutral, PublicKeyToken=null
// MVID: 13381DB9-8FD8-4EBB-8CED-9CF82DC89291
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Terraria\TerrariaServer.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
  public class HairShaderData : ShaderData
  {
    protected Vector3 _uColor = Vector3.One;
    protected Vector3 _uSecondaryColor = Vector3.One;
    protected float _uSaturation = 1f;
    protected float _uOpacity = 1f;
    protected Ref<Texture2D> _uImage;
    protected bool _shaderDisabled;

    public bool ShaderDisabled
    {
      get
      {
        return this._shaderDisabled;
      }
    }

    public HairShaderData(Ref<Effect> shader, string passName)
      : base(shader, passName)
    {
    }

    public virtual void Apply(Player player, DrawData? drawData = null)
    {
      if (this._shaderDisabled)
        return;
      this.Shader.Parameters["uColor"].SetValue(this._uColor);
      this.Shader.Parameters["uSaturation"].SetValue(this._uSaturation);
      this.Shader.Parameters["uSecondaryColor"].SetValue(this._uSecondaryColor);
      this.Shader.Parameters["uTime"].SetValue(Main.GlobalTime);
      this.Shader.Parameters["uOpacity"].SetValue(this._uOpacity);
      if (drawData.HasValue)
      {
        DrawData drawData1 = drawData.Value;
        this.Shader.Parameters["uSourceRect"].SetValue(new Vector4((float) drawData1.sourceRect.Value.X, (float) drawData1.sourceRect.Value.Y, (float) drawData1.sourceRect.Value.Width, (float) drawData1.sourceRect.Value.Height));
        this.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + drawData1.position);
        this.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float) drawData1.texture.Width, (float) drawData1.texture.Height));
      }
      else
        this.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0.0f, 0.0f, 4f, 4f));
      if (this._uImage != null)
      {
        Main.graphics.GraphicsDevice.Textures[1] = (Texture) this._uImage.Value;
        this.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float) this._uImage.Value.Width, (float) this._uImage.Value.Height));
      }
      if (player != null)
        this.Shader.Parameters["uDirection"].SetValue((float) player.direction);
      this.Apply();
    }

    public virtual Color GetColor(Player player, Color lightColor)
    {
      return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
    }

    public HairShaderData UseColor(float r, float g, float b)
    {
      return this.UseColor(new Vector3(r, g, b));
    }

    public HairShaderData UseColor(Color color)
    {
      return this.UseColor(color.ToVector3());
    }

    public HairShaderData UseColor(Vector3 color)
    {
      this._uColor = color;
      return this;
    }

    public HairShaderData UseImage(string path)
    {
      this._uImage = TextureManager.AsyncLoad(path);
      return this;
    }

    public HairShaderData UseOpacity(float alpha)
    {
      this._uOpacity = alpha;
      return this;
    }

    public HairShaderData UseSecondaryColor(float r, float g, float b)
    {
      return this.UseSecondaryColor(new Vector3(r, g, b));
    }

    public HairShaderData UseSecondaryColor(Color color)
    {
      return this.UseSecondaryColor(color.ToVector3());
    }

    public HairShaderData UseSecondaryColor(Vector3 color)
    {
      this._uSecondaryColor = color;
      return this;
    }

    public HairShaderData UseSaturation(float saturation)
    {
      this._uSaturation = saturation;
      return this;
    }
  }
}
